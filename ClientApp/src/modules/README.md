# Modules

This folder contains UI feature modules. Each module is a self-contained folder with its own React components, styles, and optional tests.

Conventions
- Module folder name: PascalCase or kebab-case (e.g. `Product` or `orders`).
- Main component: `ModuleName.tsx` (default export).
- Module CSS: `ModuleName.css` (scoped styles for the module).
- Public API: the module should export a single default React component.

Example layout

```
src/modules/Product/
  Product.tsx         # default export React component
  Product.css         # module-specific styles
  index.ts            # optional re-export
  README.md           # module notes
```

Adding a new module
1. Run the scaffolding helper from `ClientApp` (optional):

```powershell
cd ClientApp
npm run create-module -- Product
```

2. Or manually create a folder under `src/modules/` and add the files shown above.

3. Import the module in `src/App.tsx`:

```tsx
import ProductModule from './modules/Product/Product';
...
<ProductModule />
```

Notes
- Module styles are kept local to avoid leaking rules across the app.
- For code-splitting, use `React.lazy()` with `Suspense` when importing modules.
