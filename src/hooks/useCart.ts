import { useEffect } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { RootState } from '../store/store';
import { 
  fetchCart, 
  addToCart, 
  updateCartItemQuantity, 
  removeFromCart, 
  syncLocalCartWithServer,
  setAuthenticated,
  initializeCart
} from '../store/slices/cartSlice';

export const useCart = () => {
  const dispatch = useDispatch();
  const cart = useSelector((state: RootState) => state.cart);

  // Ініціалізація корзини при завантаженні
  useEffect(() => {
    dispatch(initializeCart());
  }, [dispatch]);

  // Функція для додавання товару в корзину
  const handleAddToCart = (product: {
    id: number;
    name: string;
    price: number;
    imageName: string;
    categoryName: string;
    sizeName: string;
    quantity?: number;
  }) => {
    dispatch(addToCart(product));
  };

  // Функція для збільшення кількості товару
  const handleIncreaseQuantity = (itemId: number) => {
    const item = cart.items.find(item => item.id === itemId);
    if (item) {
      dispatch(updateCartItemQuantity({ itemId, quantity: item.quantity + 1 }));
    }
  };

  // Функція для зменшення кількості товару
  const handleDecreaseQuantity = (itemId: number) => {
    const item = cart.items.find(item => item.id === itemId);
    if (item && item.quantity > 1) {
      dispatch(updateCartItemQuantity({ itemId, quantity: item.quantity - 1 }));
    } else if (item && item.quantity === 1) {
      // Якщо кількість 1, то видаляємо товар
      dispatch(removeFromCart(itemId));
    }
  };

  // Функція для видалення товару з корзини
  const handleRemoveFromCart = (itemId: number) => {
    dispatch(removeFromCart(itemId));
  };

  // Функція для синхронізації локальної корзини з сервером при логіні
  const handleLogin = async () => {
    dispatch(setAuthenticated(true));
    await dispatch(syncLocalCartWithServer());
    dispatch(fetchCart());
  };

  // Функція для виходу з акаунту
  const handleLogout = () => {
    dispatch(setAuthenticated(false));
    dispatch(initializeCart());
  };

  // Функція для завантаження корзини
  const loadCart = () => {
    dispatch(fetchCart());
  };

  return {
    cart,
    addToCart: handleAddToCart,
    increaseQuantity: handleIncreaseQuantity,
    decreaseQuantity: handleDecreaseQuantity,
    removeFromCart: handleRemoveFromCart,
    onLogin: handleLogin,
    onLogout: handleLogout,
    loadCart,
  };
}; 