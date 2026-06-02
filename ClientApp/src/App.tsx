import ProductModule from './modules/Product/Product';
import { swaggerUrl } from './api';
import './styles';

function App() {
  const openSwagger = () => window.open(swaggerUrl, '_blank');

  return (
    <div className="app-shell">
      <header className="hero">
        <div>
          <p className="eyebrow">Sample App</p>
          <h1>Product Manager</h1>
          <p className="hero-copy">Create, update, and delete inventory items with a modern React frontend backed by your ASP.NET Core API.</p>
        </div>
        <div className="hero-badge">
          Connected to API
          <button type="button" className="mini" onClick={openSwagger} style={{ marginLeft: 12 }}>
            Open API (Swagger)
          </button>
        </div>
      </header>

      <main>
        <ProductModule />
      </main>
    </div>
  );
}

export default App;
