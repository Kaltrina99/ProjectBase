import React, { useEffect, useState } from 'react';
import { Product as ProductModel, CreateProductRequest, UpdateProductRequest } from '../../types';
import { fetchProducts, createProduct, updateProduct, deleteProduct } from '../../api';
import './Product.css';

const Product: React.FC = () => {
  const [products, setProducts] = useState<ProductModel[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const [name, setName] = useState('');
  const [price, setPrice] = useState('');
  const [description, setDescription] = useState('');
  const [editingId, setEditingId] = useState<string | null>(null);

  useEffect(() => {
    loadProducts();
  }, []);

  const loadProducts = async () => {
    setLoading(true);
    setError(null);

    try {
      const result = await fetchProducts();
      setProducts(result);
    } catch (err) {
      setError('Unable to load products.');
    } finally {
      setLoading(false);
    }
  };

  const resetForm = () => {
    setName('');
    setPrice('');
    setDescription('');
    setEditingId(null);
  };

  const handleSubmit = async (event: React.FormEvent<HTMLFormElement>) => {
    event.preventDefault();
    setError(null);

    const priceValue = parseFloat(price);
    if (!name.trim() || Number.isNaN(priceValue) || priceValue < 0) {
      setError('Please provide a valid product name and price.');
      return;
    }

    const request: CreateProductRequest | UpdateProductRequest = {
      name: name.trim(),
      price: priceValue,
      description: description.trim() || undefined
    };

    try {
      if (editingId) {
        await updateProduct(editingId, request as UpdateProductRequest);
      } else {
        await createProduct(request as CreateProductRequest);
      }

      resetForm();
      await loadProducts();
    } catch (err) {
      setError(editingId ? 'Unable to update product.' : 'Unable to create product.');
    }
  };

  const handleEdit = (product: ProductModel) => {
    setEditingId(product.id);
    setName(product.name);
    setPrice(product.price.toString());
    setDescription(product.description ?? '');
  };

  const handleDelete = async (id: string) => {
    setError(null);

    try {
      await deleteProduct(id);
      await loadProducts();
    } catch (err) {
      setError(err instanceof Error ? err.message : 'Unable to delete product.');
    }
  };

  return (
    <div className="main-grid">
      <section className="form-panel">
        <div className="panel-header">
          <div>
            <h2>{editingId ? 'Edit product' : 'Add a product'}</h2>
            <p>Use this form to add a new item or update an existing one.</p>
          </div>
        </div>

        <form onSubmit={handleSubmit}>
          <label>
            Product name
            <input value={name} onChange={(e) => setName(e.target.value)} placeholder="Enter product name" required />
          </label>

          <label>
            Price
            <input
              value={price}
              onChange={(e) => setPrice(e.target.value)}
              type="number"
              step="0.01"
              min="0"
              placeholder="0.00"
              required
            />
          </label>

          <label>
            Description
            <input value={description} onChange={(e) => setDescription(e.target.value)} placeholder="Optional description" />
          </label>

          <div className="button-group">
            <button type="submit" disabled={loading}>
              {editingId ? 'Save changes' : 'Create product'}
            </button>
            {editingId ? (
              <button type="button" className="secondary" onClick={resetForm}>
                Cancel
              </button>
            ) : null}
          </div>
        </form>
      </section>

      <section className="products-panel">
        <div className="panel-header">
          <div>
            <h2>Products</h2>
            <p>Manage records currently stored in the API.</p>
          </div>
        </div>

        {loading ? (
          <div className="loading-card">Loading products...</div>
        ) : error ? (
          <div className="error-card">{error}</div>
        ) : products.length === 0 ? (
          <div className="empty-state">No products found. Use the form to add one.</div>
        ) : (
          <ul className="products-list">
            {products.map((product) => (
              <li key={product.id} className="product-item">
                <div className="product-meta">
                  <div>
                    <h3>{product.name}</h3>
                    <p>{product.description || 'No description provided.'}</p>
                  </div>
                  <div className="actions">
                    <button type="button" onClick={() => handleEdit(product)}>
                      Edit
                    </button>
                    <button type="button" className="danger" onClick={() => handleDelete(product.id)}>
                      Delete
                    </button>
                  </div>
                </div>
                <div className="product-footer">
                  <span>${product.price.toFixed(2)}</span>
                  <span>{new Date(product.createdAt).toLocaleString()}</span>
                </div>
              </li>
            ))}
          </ul>
        )}
      </section>
    </div>
  );
};

export default Product;
