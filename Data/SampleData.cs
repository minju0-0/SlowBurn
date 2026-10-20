using SlowBurn.Models;

namespace SlowBurn.Data;

/// <summary>Static data built from the model classes (no database).</summary>
public static class SampleData
{
    public static List<Product> Products { get; } = new()
    {
        // --- Single Origin Coffees (Packaged Whole Bean Bags) ---
        new Product 
        { 
            Id = 1, Sku = "SB-014", Name = "Guji Natural", Origin = "Ethiopia", Category = ProductCategory.Beans, Roast = RoastLevel.Light,
            Price = 18.50m, Stock = 24, IsNew = true, Art = "bag", Tone = "#b0321a",
            Notes = "Blueberry jam, jasmine, brown sugar",
            Description = "Sweet and loud. Stand-up craft bag of heirloom varieties dried on raised African beds.",
            Details = { "250 g craft pouch", "Altitude 2,100 m", "Natural process, whole bean" },
            ImageUrl = "images/products/guji-natural.jpg"
        },
        new Product 
        { 
            Id = 2, Sku = "SB-021", Name = "Huila Washed", Origin = "Colombia", Category = ProductCategory.Beans, Roast = RoastLevel.Medium,
            Price = 16.00m, Stock = 40, Art = "bag", Tone = "#c98a2b",
            Notes = "Red apple, panela, cocoa nib",
            Description = "The daily driver. Clean white resealable bag of balanced Caturra and Castillo roast.",
            Details = { "250 g white pouch", "Altitude 1,750 m", "Fully washed, whole bean" },
            ImageUrl = "images/products/huila-washed.jpg"
        },
        new Product 
        { 
            Id = 3, Sku = "SB-027", Name = "Cerrado Pulped Natural", Origin = "Brazil", Category = ProductCategory.Beans, Roast = RoastLevel.Medium,
            Price = 14.50m, Stock = 31, Art = "bag", Tone = "#7b2d12",
            Notes = "Hazelnut, milk chocolate, low acidity",
            Description = "Traditional kraft paper valve pouch. Creamy body and sweet chocolate aroma for espresso and filter.",
            Details = { "250 g kraft valve bag", "Altitude 1,100 m", "Pulped natural, yellow bourbon" },
            ImageUrl = "images/products/cerrado-brazil.jpg"
        },
        new Product 
        { 
            Id = 4, Sku = "SB-033", Name = "Nyeri AA", Origin = "Kenya", Category = ProductCategory.Beans, Roast = RoastLevel.Light,
            Price = 21.00m, Stock = 4, Art = "bag", Tone = "#2a4a5e",
            Notes = "Blackcurrant, grapefruit, cane sugar",
            Description = "Artisan single-origin batch. Vibrant acidity, fruity sweetness, and prominent AA screen size.",
            Details = { "250 g artisan bag", "Altitude 1,800 m", "Washed, SL28 and SL34" },
            ImageUrl = "images/products/nyeri-aa.jpg"
        },
        new Product 
        { 
            Id = 5, Sku = "SB-040", Name = "Mandheling Dark Roast", Origin = "Sumatra", Category = ProductCategory.Beans, Roast = RoastLevel.Dark,
            Price = 15.00m, Stock = 18, Art = "bag", Tone = "#3b3b36",
            Notes = "Dark cocoa, cedar, brown spice",
            Description = "Heavy and bold. Packed in a matte black degassing bag to lock in maximum freshness.",
            Details = { "250 g matte pouch", "Altitude 1,400 m", "Wet-hulled (Giling Basah)" },
            ImageUrl = "images/products/mandheling-sumatra.jpg"
        },

        // --- Brewing Gear ---
        new Product 
        { 
            Id = 6, Sku = "GR-101", Name = "Ceramic Cone Dripper", Category = ProductCategory.Gear,
            Price = 28.00m, Stock = 15, Art = "cone", Tone = "#c98a2b",
            Notes = "Size 02, brews 1 to 4 cups",
            Description = "Classic ceramic cone with 60-degree spiral ribs for optimal water extraction flow.",
            Details = { "White glazed ceramic", "Fits size 02 cone filters", "Dishwasher safe" },
            ImageUrl = "images/products/ceramic-dripper.jpg"
        },
        new Product 
        { 
            Id = 7, Sku = "GR-108", Name = "Gooseneck Kettle 0.9 L", Category = ProductCategory.Gear,
            Price = 54.00m, Stock = 9, Art = "kettle", Tone = "#3b3b36",
            Notes = "Narrow spout for consistent flow rate",
            Description = "Matte black stainless steel kettle with an ergonomic counterbalanced handle and precision pour spout.",
            Details = { "Stainless steel 0.9 L", "Induction & gas compatible", "Built-in thermometer lid" },
            ImageUrl = "images/products/gooseneck-kettle.jpg"
        },
        new Product 
        { 
            Id = 8, Sku = "GR-115", Name = "Precision Coffee Scale", Category = ProductCategory.Gear,
            Price = 32.00m, Stock = 12, Art = "scale", Tone = "#2f5d3a",
            Notes = "0.1 g precision with integrated timer",
            Description = "Fast-response digital timer scale for dialing in exact coffee-to-water ratios.",
            Details = { "2 kg capacity, 0.1 g resolution", "Auto-tare & auto-timer", "Type-C rechargeable" },
            ImageUrl = "images/products/coffee-scale.jpg"
        },
        new Product 
        { 
            Id = 9, Sku = "GR-122", Name = "Stainless Hand Grinder", Category = ProductCategory.Gear,
            Price = 79.00m, Stock = 8, Art = "grinder", Tone = "#b0321a",
            Notes = "CNC stainless conical burrs, 24 clicks",
            Description = "Compact manual grinder with dual bearings for effortless, uniform particle sizing anywhere.",
            Details = { "38 mm CNC conical burrs", "24 stepped grind settings", "30 g hopper capacity" },
            ImageUrl = "images/products/hand-grinder.jpg"
        },

        // --- Bundles ---
        new Product 
        { 
            Id = 10, Sku = "BN-201", Name = "Pour-over Starter Set", Category = ProductCategory.Bundles,
            Price = 69.00m, Stock = 6, IsNew = true, Art = "box", Tone = "#7b2d12",
            Notes = "Dripper, filters, server, and beans",
            Description = "The complete home barista package. Everything needed to brew cafe-quality pour-overs on day one.",
            Details = { "Ceramic cone dripper", "100 bleached paper filters", "250 g Huila Washed beans" },
            ImageUrl = "images/products/pourover-starter.jpg"
        },
        new Product 
        { 
            Id = 11, Sku = "BN-205", Name = "Three-Origin Tasting Flight", Category = ProductCategory.Bundles,
            Price = 44.00m, Stock = 14, Art = "box", Tone = "#2a4a5e",
            Notes = "Three 100 g bags: Ethiopia, Colombia, Brazil",
            Description = "Compare three distinct processing methods and roasting profiles side by side.",
            Details = { "3 x 100 g sample bags", "Tasting wheel card included", "Roasted on the same day" },
            ImageUrl = "images/products/three-origin-flight.jpg"
        },
    };

    public static List<BrewMethod> BrewMethods { get; } = new()
    {
        new BrewMethod 
        { 
            Name = "Pour-over", Tagline = "Clean and bright", Ratio = 16, TempC = 94, Seconds = 180, MlPerServing = 250, MaxServings = 6, Unit = "cup", Grind = GrindSize.Filter,
            Steps = { "Rinse the paper filter with hot water and discard the rinse.",
                      "Add the ground coffee, level the bed and zero the scale.",
                      "Bloom: pour twice the coffee's weight in water and wait 40 seconds.",
                      "Pour in slow circles until you reach the full water weight by 1:45.",
                      "Let it drain, finishing near 3:00. Swirl and serve." } 
        },
        new BrewMethod 
        { 
            Name = "French press", Tagline = "Full body, no fuss", Ratio = 15, TempC = 96, Seconds = 240, MlPerServing = 250, MaxServings = 6, Unit = "cup", Grind = GrindSize.Coarse,
            Steps = { "Warm the press with hot water, then empty it.",
                      "Add coarse coffee, pour all the water and start the timer.",
                      "At 4:00 break the crust with a spoon and skim the foam.",
                      "Press gently and pour straight away so it doesn't over-extract." } 
        },
        new BrewMethod 
        { 
            Name = "Espresso", Tagline = "Short, sweet, syrupy", Ratio = 2, TempC = 93, Seconds = 28, MlPerServing = 36, MaxServings = 4, Unit = "shot", Grind = GrindSize.Espresso,
            Steps = { "Purge the group head and dry the portafilter basket.",
                      "Dose, level and tamp flat with firm, even pressure.",
                      "Start the shot and watch for the first drip at about 6 seconds.",
                      "Stop at the target weight. Too fast means grind finer, too slow means coarser." } 
        },
        new BrewMethod 
        { 
            Name = "AeroPress", Tagline = "Forgiving and fast", Ratio = 12, TempC = 85, Seconds = 120, MlPerServing = 200, MaxServings = 4, Unit = "cup", Grind = GrindSize.Filter,
            Steps = { "Set it up inverted with a rinsed filter in the cap.",
                      "Add coffee, pour the water and stir ten times.",
                      "Steep until 1:30, then screw on the cap.",
                      "Flip onto your mug and press slowly for 30 seconds." } 
        },
    };
}