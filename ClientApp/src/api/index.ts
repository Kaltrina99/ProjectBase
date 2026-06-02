import { CreateProductRequest, Product, UpdateProductRequest } from '../types';

const baseUrl = '/api/Products';

// Vite exposes env vars via `import.meta.env`. Use `VITE_SWAGGER_URL` when provided
// (e.g. VITE_SWAGGER_URL=/swagger/index.html or full URL). Fallback to localhost:5010.
const envSwagger = (import.meta as any).env?.VITE_SWAGGER_URL;
export const swaggerUrl = envSwagger
  ? envSwagger.startsWith('http')
    ? envSwagger
    : `${window.location.protocol}//${window.location.hostname}:${envSwagger.startsWith('/') ? '' : ''}${envSwagger}`
  : `${window.location.protocol}//${window.location.hostname}:5010/swagger/index.html`;

async function handleResponse<T>(response: Response): Promise<T> {
  if (!response.ok) {
    throw new Error(`API request failed with status ${response.status}`);
  }
  return response.json();
}

export async function fetchProducts(): Promise<Product[]> {
  const response = await fetch(baseUrl, {
    headers: {
      Accept: 'application/json'
    }
  });

  return handleResponse<Product[]>(response);
}

export async function createProduct(request: CreateProductRequest): Promise<Product> {
  const response = await fetch(baseUrl, {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json',
      Accept: 'application/json'
    },
    body: JSON.stringify(request)
  });

  return handleResponse<Product>(response);
}

export async function updateProduct(id: string, request: UpdateProductRequest): Promise<Product> {
  const response = await fetch(`${baseUrl}/${id}`, {
    method: 'PUT',
    headers: {
      'Content-Type': 'application/json',
      Accept: 'application/json'
    },
    body: JSON.stringify(request)
  });

  return handleResponse<Product>(response);
}

export async function deleteProduct(id: string): Promise<void> {
  const response = await fetch(`${baseUrl}/${id}`, {
    method: 'DELETE',
    headers: {
      Accept: 'application/json'
    }
  });

  if (response.status === 204) return;

  if (!response.ok) {
    const text = await response.text();
    const body = text && text.length > 0 ? text : undefined;
    throw new Error(body ?? `Failed to delete product with status ${response.status}`);
  }
}
