export interface CartItemModel {
  id: number;
  productId: number;
  categoryId: number;
  name: string;
  categoryName: string;
  quantity: number;
  price: number;
  sizeName: string;
  imageName: string;
}

export interface CartListModel {
  items: CartItemModel[];
  totalPrice: number;
}

export interface CartItemCreateModel {
  productId: number;
  quantity: number;
} 