-- SlowBurn · database/schema.sql
-- Supabase Dashboard -> SQL Editor -> New query -> paste this whole file -> Run.
-- Safe to re-run: it drops and recreates the tables (this deletes their data).

drop table if exists public.order_items;
drop table if exists public.orders;
drop table if exists public.products;
drop table if exists public.brew_methods;
drop table if exists public.contact_messages;

-- ===================== PRODUCTS (storage pattern: SOFT DELETE) =====================
create table public.products (
  id          integer generated always as identity primary key,
  sku         text not null unique,
  name        text not null check (length(trim(name)) > 0),
  origin      text not null default '',
  notes       text not null default '',
  description text not null default '',
  price       numeric(10,2) not null default 0 check (price >= 0),
  stock       integer not null default 0 check (stock >= 0),
  is_new      boolean not null default false,
  category    text not null check (category in ('Beans','Gear','Bundles')),
  roast       text not null default 'None' check (roast in ('None','Light','Medium','Dark')),
  art         text not null default 'bag',
  tone        text not null default '#7b2d12',
  image_url   text,
  details     text[] not null default '{}',
  deleted_at  timestamptz,                       -- soft delete: filled in instead of removing the row
  created_at  timestamptz not null default now()
);
create index on public.products (name);          -- habit 1: fast search
create index on public.products (category);      -- habit 1: fast category filter

-- ===================== ORDERS (storage pattern: APPEND-ONLY LOG) =====================
create table public.orders (
  reference   text primary key,                  -- e.g. SB-482913
  full_name   text not null check (length(trim(full_name)) > 0),
  email       text not null,
  phone       text not null,
  delivery    text not null check (delivery in ('Standard','Express','Pickup')),
  address     text,
  city        text,
  postal_code text,
  note        text not null default '',
  item_count  integer not null check (item_count > 0),
  total       numeric(10,2) not null check (total >= 0),
  created_at  timestamptz not null default now(),
  -- same rule as your CheckoutForm: an address is required unless the customer picks Pickup
  constraint address_needed_unless_pickup check (
    delivery = 'Pickup'
    or (length(trim(coalesce(address, ''))) > 0
        and length(trim(coalesce(city, ''))) > 0
        and length(trim(coalesce(postal_code, ''))) > 0)
  )
);
create index on public.orders (email);

create table public.order_items (
  id              integer generated always as identity primary key,
  order_reference text not null references public.orders(reference),   -- FK: many items belong to one order
  product_id      integer not null references public.products(id),     -- FK: points at the product bought
  quantity        integer not null check (quantity > 0),
  grind           text,
  unit_price      numeric(10,2) not null check (unit_price >= 0)       -- price at the time of purchase
);
create index on public.order_items (order_reference);
create index on public.order_items (product_id);

-- ===================== BREW METHODS (storage pattern: UPDATE IN PLACE) =====================
create table public.brew_methods (
  id             integer generated always as identity primary key,
  name           text not null unique,
  tagline        text not null default '',
  ratio          numeric(5,2) not null check (ratio > 0),     -- grams of water per gram of coffee
  temp_c         integer not null check (temp_c between 0 and 100),
  seconds        integer not null check (seconds > 0),
  ml_per_serving integer not null check (ml_per_serving > 0),
  max_servings   integer not null check (max_servings > 0),
  unit           text not null default 'cup',
  grind          text not null check (grind in ('WholeBean','Espresso','Filter','Coarse')),
  steps          text[] not null default '{}',
  sort_order     integer not null default 0
);

-- ===================== CONTACT MESSAGES (storage pattern: APPEND-ONLY LOG) =====================
create table public.contact_messages (
  id         integer generated always as identity primary key,
  name       text not null check (length(trim(name)) > 0),
  email      text not null,
  message    text not null check (length(trim(message)) >= 10),
  created_at timestamptz not null default now()
);

-- ===================== LOCK 1: GRANT =====================
grant select, insert, update, delete on public.products to anon;
grant insert on public.orders      to anon;      -- insert only: the public key must NOT be able to read customer emails
grant insert on public.order_items to anon;
grant select on public.brew_methods to anon;          -- read only: visitors cannot change brew guides
grant insert on public.contact_messages to anon;      -- insert only: visitors cannot read other people's messages

-- ===================== LOCK 2: ROW LEVEL SECURITY =====================
alter table public.products    enable row level security;
alter table public.orders      enable row level security;
alter table public.order_items enable row level security;
alter table public.brew_methods enable row level security;
alter table public.contact_messages enable row level security;

create policy "read_all"   on public.products for select to anon using (true);
create policy "insert_all" on public.products for insert to anon with check (true);
create policy "update_all" on public.products for update to anon using (true) with check (true);
create policy "delete_all" on public.products for delete to anon using (true);

create policy "insert_orders"      on public.orders      for insert to anon with check (true);
create policy "insert_order_items" on public.order_items for insert to anon with check (true);
create policy "read_brew_methods"  on public.brew_methods     for select to anon using (true);
create policy "insert_contact"      on public.contact_messages for insert to anon with check (true);
-- NOTE: these open policies are for class only. A real shop would require sign-in.

-- ===================== SEED DATA (from Data/SampleData.cs) =====================
insert into public.products
  (sku, name, origin, notes, description, price, stock, is_new, category, roast, art, tone, image_url, details)
values
  ('SB-014', 'Guji Natural', 'Ethiopia', 'Blueberry jam, jasmine, brown sugar', 'Sweet and loud. Stand-up craft bag of heirloom varieties dried on raised African beds.', 18.50, 24, true, 'Beans', 'Light', 'bag', '#b0321a', 'images/products/guji-natural.jpg', array['250 g craft pouch', 'Altitude 2,100 m', 'Natural process, whole bean']),
  ('SB-021', 'Huila Washed', 'Colombia', 'Red apple, panela, cocoa nib', 'The daily driver. Clean white resealable bag of balanced Caturra and Castillo roast.', 16.00, 40, false, 'Beans', 'Medium', 'bag', '#c98a2b', 'images/products/huila-washed.jpg', array['250 g white pouch', 'Altitude 1,750 m', 'Fully washed, whole bean']),
  ('SB-027', 'Cerrado Pulped Natural', 'Brazil', 'Hazelnut, milk chocolate, low acidity', 'Traditional kraft paper valve pouch. Creamy body and sweet chocolate aroma for espresso and filter.', 14.50, 31, false, 'Beans', 'Medium', 'bag', '#7b2d12', 'images/products/cerrado-brazil.jpg', array['250 g kraft valve bag', 'Altitude 1,100 m', 'Pulped natural, yellow bourbon']),
  ('SB-033', 'Nyeri AA', 'Kenya', 'Blackcurrant, grapefruit, cane sugar', 'Artisan single-origin batch. Vibrant acidity, fruity sweetness, and prominent AA screen size.', 21.00, 4, false, 'Beans', 'Light', 'bag', '#2a4a5e', 'images/products/nyeri-aa.jpg', array['250 g artisan bag', 'Altitude 1,800 m', 'Washed, SL28 and SL34']),
  ('SB-040', 'Mandheling Dark Roast', 'Sumatra', 'Dark cocoa, cedar, brown spice', 'Heavy and bold. Packed in a matte black degassing bag to lock in maximum freshness.', 15.00, 18, false, 'Beans', 'Dark', 'bag', '#3b3b36', 'images/products/mandheling-sumatra.jpg', array['250 g matte pouch', 'Altitude 1,400 m', 'Wet-hulled (Giling Basah)']),
  ('GR-101', 'Ceramic Cone Dripper', '', 'Size 02, brews 1 to 4 cups', 'Classic ceramic cone with 60-degree spiral ribs for optimal water extraction flow.', 28.00, 15, false, 'Gear', 'None', 'cone', '#c98a2b', 'images/products/ceramic-dripper.jpg', array['White glazed ceramic', 'Fits size 02 cone filters', 'Dishwasher safe']),
  ('GR-108', 'Gooseneck Kettle 0.9 L', '', 'Narrow spout for consistent flow rate', 'Matte black stainless steel kettle with an ergonomic counterbalanced handle and precision pour spout.', 54.00, 9, false, 'Gear', 'None', 'kettle', '#3b3b36', 'images/products/gooseneck-kettle.jpg', array['Stainless steel 0.9 L', 'Induction & gas compatible', 'Built-in thermometer lid']),
  ('GR-115', 'Precision Coffee Scale', '', '0.1 g precision with integrated timer', 'Fast-response digital timer scale for dialing in exact coffee-to-water ratios.', 32.00, 12, false, 'Gear', 'None', 'scale', '#2f5d3a', 'images/products/coffee-scale.jpg', array['2 kg capacity, 0.1 g resolution', 'Auto-tare & auto-timer', 'Type-C rechargeable']),
  ('GR-122', 'Stainless Hand Grinder', '', 'CNC stainless conical burrs, 24 clicks', 'Compact manual grinder with dual bearings for effortless, uniform particle sizing anywhere.', 79.00, 8, false, 'Gear', 'None', 'grinder', '#b0321a', 'images/products/hand-grinder.jpg', array['38 mm CNC conical burrs', '24 stepped grind settings', '30 g hopper capacity']),
  ('BN-201', 'Pour-over Starter Set', '', 'Dripper, filters, server, and beans', 'The complete home barista package. Everything needed to brew cafe-quality pour-overs on day one.', 69.00, 6, true, 'Bundles', 'None', 'box', '#7b2d12', 'images/products/pourover-starter.jpg', array['Ceramic cone dripper', '100 bleached paper filters', '250 g Huila Washed beans']),
  ('BN-205', 'Three-Origin Tasting Flight', '', 'Three 100 g bags: Ethiopia, Colombia, Brazil', 'Compare three distinct processing methods and roasting profiles side by side.', 44.00, 14, false, 'Bundles', 'None', 'box', '#2a4a5e', 'images/products/three-origin-flight.jpg', array['3 x 100 g sample bags', 'Tasting wheel card included', 'Roasted on the same day']);

insert into public.brew_methods
  (name, tagline, ratio, temp_c, seconds, ml_per_serving, max_servings, unit, grind, steps, sort_order)
values
  ('Pour-over', 'Clean and bright', 16, 94, 180, 250, 6, 'cup', 'Filter', array['Rinse the paper filter with hot water and discard the rinse.', 'Add the ground coffee, level the bed and zero the scale.', 'Bloom: pour twice the coffee''s weight in water and wait 40 seconds.', 'Pour in slow circles until you reach the full water weight by 1:45.', 'Let it drain, finishing near 3:00. Swirl and serve.'], 1),
  ('French press', 'Full body, no fuss', 15, 96, 240, 250, 6, 'cup', 'Coarse', array['Warm the press with hot water, then empty it.', 'Add coarse coffee, pour all the water and start the timer.', 'At 4:00 break the crust with a spoon and skim the foam.', 'Press gently and pour straight away so it doesn''t over-extract.'], 2),
  ('Espresso', 'Short, sweet, syrupy', 2, 93, 28, 36, 4, 'shot', 'Espresso', array['Purge the group head and dry the portafilter basket.', 'Dose, level and tamp flat with firm, even pressure.', 'Start the shot and watch for the first drip at about 6 seconds.', 'Stop at the target weight. Too fast means grind finer, too slow means coarser.'], 3),
  ('AeroPress', 'Forgiving and fast', 12, 85, 120, 200, 4, 'cup', 'Filter', array['Set it up inverted with a rinsed filter in the cap.', 'Add coffee, pour the water and stir ten times.', 'Steep until 1:30, then screw on the cap.', 'Flip onto your mug and press slowly for 30 seconds.'], 4);