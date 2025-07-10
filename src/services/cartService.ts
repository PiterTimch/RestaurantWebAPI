import { CartListModel, CartItemCreateModel } from '../types/cart';
import { apiClient } from './apiClient';

class CartService {
  async getCart(): Promise<CartListModel> {
    const response = await apiClient.get('/api/cart/getCart');
    return response.data;
  }

  async createUpdate(model: CartItemCreateModel): Promise<void> {
    await apiClient.post('/api/cart/createUpdate', model);
  }

  async removeCartItem(cartItemId: number): Promise<void> {
    await apiClient.put(`/api/cart/removeCartItem/${cartItemId}`);
  }
}

export const cartService = new CartService(); 