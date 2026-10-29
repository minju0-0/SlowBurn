# SlowBurn (Blazor .NET 8 + Tailwind v4)

An artisan mail-order coffee roastery web application featuring small-lot single-origin beans, brewing gear, brew guide dial-in calculator, contact counter, and checkout.

## Architecture & File Structure
- `Services/` &mdash; Dependency-injected application services:
  - `ProductService` (`IProductService`): Catalog queries, searching, multi-criteria filtering, and sorting algorithms.
  - `CartService` (`ICartService`): Scoped reactive shopping cart, stock capping, free-shipping threshold, and promo calculations.
  - `OrderService` (`IOrderService`): Asynchronous order dispatch, receipt generation, and cart lifecycle.
- `Models/` &mdash; Clean domain entities and data-annotated forms:
  - `Models/Entities/`: `Product.cs`, `BrewMethod.cs`, `CartItem.cs`, `OrderReceipt.cs`.
  - `Models/Forms/`: `CheckoutForm.cs` (with conditional delivery validation), `ContactForm.cs`.
  - `Models/Enums.cs`: Strongly-typed domain enums.
- `Common/` &mdash; Shared decoupled presentation formatting (`Money.cs`, `Labels.cs`).
- `Components/` &mdash; Modern Razor components:
  - `Pages/`: `Home.razor`, `Shop.razor`, `Brew.razor`, `Contact.razor`, `Checkout.razor`.
  - `Layout/`: `MainLayout.razor`, `NavMenu.razor`.
  - `Shared/`: `ProductArt.razor` (responsive vector art illustrations).
- `Data/` &mdash; `SampleData.cs` catalog definitions.
- `Styles/` &mdash; Tailwind v4 `@theme` craft tokens and custom halftone surfaces.

## Run
```bash
npm install
npm run build:css      # or: npm run watch:css (second terminal)
dotnet run             # or: dotnet watch
```
