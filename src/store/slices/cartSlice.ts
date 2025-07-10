import { createSlice, createAsyncThunk, PayloadAction } from '@reduxjs/toolkit';
import { CartItemModel, CartListModel } from '../../types/cart';
import { cartService } from '../../services/cartService';

// Типи для локального зберігання
interface LocalCartItem {
  productId: number;
  quantity: number;
  name: string;
  price: number;
  imageName: string;
  categoryName: string;
  sizeName: string;
}

interface CartState {
  items: CartItemModel[];
  totalPrice: number;
  isLoading: boolean;
  error: string | null;
  isAuthenticated: boolean;
}

const LOCAL_CART_KEY = 'localCart';

// Функція для збереження локальної корзини
const saveLocalCart = (items: LocalCartItem[]) => {
  localStorage.setItem(LOCAL_CART_KEY, JSON.stringify(items));
};

// Функція для завантаження локальної корзини
const loadLocalCart = (): LocalCartItem[] => {
  const saved = localStorage.getItem(LOCAL_CART_KEY);
  return saved ? JSON.parse(saved) : [];
};

// Функція для конвертації локальних елементів в CartItemModel
const convertLocalToCartItems = (localItems: LocalCartItem[]): CartItemModel[] => {
  return localItems.map((item, index) => ({
    id: -(index + 1), // Негативні ID для локальних елементів
    productId: item.productId,
    quantity: item.quantity,
    name: item.name,
    price: item.price,
    imageName: item.imageName,
    categoryName: item.categoryName,
    sizeName: item.sizeName,
    categoryId: 0, // Буде заповнено при синхронізації
  }));
};

const initialState: CartState = {
  items: [],
  totalPrice: 0,
  isLoading: false,
  error: null,
  isAuthenticated: false,
};

// Async thunks
export const fetchCart = createAsyncThunk(
  'cart/fetchCart',
  async (_, { getState }) => {
    const state = getState() as { cart: CartState };
    
    if (state.cart.isAuthenticated) {
      // Завантажуємо з сервера для авторизованих користувачів
      const response = await cartService.getCart();
      return response;
    } else {
      // Завантажуємо з localStorage для неавторизованих
      const localItems = loadLocalCart();
      const cartItems = convertLocalToCartItems(localItems);
      const totalPrice = cartItems.reduce((sum, item) => sum + (item.price * item.quantity), 0);
      
      return {
        items: cartItems,
        totalPrice
      };
    }
  }
);

export const addToCart = createAsyncThunk(
  'cart/addToCart',
  async (product: { 
    id: number; 
    name: string; 
    price: number; 
    imageName: string; 
    categoryName: string; 
    sizeName: string;
    quantity?: number;
  }, { getState, dispatch }) => {
    const state = getState() as { cart: CartState };
    const quantity = product.quantity || 1;
    
    if (state.cart.isAuthenticated) {
      // Для авторизованих користувачів - зберігаємо на сервері
      await cartService.createUpdate({
        productId: product.id,
        quantity: quantity
      });
      
      // Оновлюємо корзину з сервера
      dispatch(fetchCart());
    } else {
      // Для неавторизованих - зберігаємо локально
      const localItems = loadLocalCart();
      const existingItemIndex = localItems.findIndex(item => item.productId === product.id);
      
      if (existingItemIndex !== -1) {
        localItems[existingItemIndex].quantity += quantity;
      } else {
        localItems.push({
          productId: product.id,
          quantity: quantity,
          name: product.name,
          price: product.price,
          imageName: product.imageName,
          categoryName: product.categoryName,
          sizeName: product.sizeName,
        });
      }
      
      saveLocalCart(localItems);
      
      return {
        items: convertLocalToCartItems(localItems),
        totalPrice: localItems.reduce((sum, item) => sum + (item.price * item.quantity), 0)
      };
    }
  }
);

export const updateCartItemQuantity = createAsyncThunk(
  'cart/updateQuantity',
  async ({ itemId, quantity }: { itemId: number; quantity: number }, { getState, dispatch }) => {
    const state = getState() as { cart: CartState };
    
    if (state.cart.isAuthenticated) {
      // Для авторизованих користувачів
      const item = state.cart.items.find(item => item.id === itemId);
      if (item) {
        await cartService.createUpdate({
          productId: item.productId,
          quantity: quantity
        });
        dispatch(fetchCart());
      }
    } else {
      // Для неавторизованих користувачів
      const localItems = loadLocalCart();
      const itemIndex = localItems.findIndex(item => item.productId === Math.abs(itemId));
      
      if (itemIndex !== -1) {
        if (quantity <= 0) {
          localItems.splice(itemIndex, 1);
        } else {
          localItems[itemIndex].quantity = quantity;
        }
        
        saveLocalCart(localItems);
        
        return {
          items: convertLocalToCartItems(localItems),
          totalPrice: localItems.reduce((sum, item) => sum + (item.price * item.quantity), 0)
        };
      }
    }
  }
);

export const removeFromCart = createAsyncThunk(
  'cart/removeFromCart',
  async (itemId: number, { getState, dispatch }) => {
    const state = getState() as { cart: CartState };
    
    if (state.cart.isAuthenticated) {
      // Для авторизованих користувачів
      await cartService.removeCartItem(itemId);
      dispatch(fetchCart());
    } else {
      // Для неавторизованих користувачів
      const localItems = loadLocalCart();
      const itemIndex = localItems.findIndex(item => item.productId === Math.abs(itemId));
      
      if (itemIndex !== -1) {
        localItems.splice(itemIndex, 1);
        saveLocalCart(localItems);
        
        return {
          items: convertLocalToCartItems(localItems),
          totalPrice: localItems.reduce((sum, item) => sum + (item.price * item.quantity), 0)
        };
      }
    }
  }
);

export const syncLocalCartWithServer = createAsyncThunk(
  'cart/syncLocalCart',
  async (_, { getState, dispatch }) => {
    const state = getState() as { cart: CartState };
    const localItems = loadLocalCart();
    
    if (localItems.length > 0) {
      // Додаємо всі локальні елементи до серверної корзини
      for (const localItem of localItems) {
        await cartService.createUpdate({
          productId: localItem.productId,
          quantity: localItem.quantity
        });
      }
      
      // Очищаємо локальну корзину
      localStorage.removeItem(LOCAL_CART_KEY);
      
      // Оновлюємо корзину з сервера
      dispatch(fetchCart());
    }
  }
);

const cartSlice = createSlice({
  name: 'cart',
  initialState,
  reducers: {
    setAuthenticated: (state, action: PayloadAction<boolean>) => {
      state.isAuthenticated = action.payload;
    },
    clearCart: (state) => {
      state.items = [];
      state.totalPrice = 0;
      localStorage.removeItem(LOCAL_CART_KEY);
    },
    initializeCart: (state) => {
      if (!state.isAuthenticated) {
        const localItems = loadLocalCart();
        const cartItems = convertLocalToCartItems(localItems);
        const totalPrice = cartItems.reduce((sum, item) => sum + (item.price * item.quantity), 0);
        
        state.items = cartItems;
        state.totalPrice = totalPrice;
      }
    },
  },
  extraReducers: (builder) => {
    builder
      // fetchCart
      .addCase(fetchCart.pending, (state) => {
        state.isLoading = true;
        state.error = null;
      })
      .addCase(fetchCart.fulfilled, (state, action) => {
        state.isLoading = false;
        state.items = action.payload.items;
        state.totalPrice = action.payload.totalPrice;
      })
      .addCase(fetchCart.rejected, (state, action) => {
        state.isLoading = false;
        state.error = action.error.message || 'Failed to fetch cart';
      })
      
      // addToCart
      .addCase(addToCart.pending, (state) => {
        state.isLoading = true;
        state.error = null;
      })
      .addCase(addToCart.fulfilled, (state, action) => {
        state.isLoading = false;
        if (action.payload) {
          state.items = action.payload.items;
          state.totalPrice = action.payload.totalPrice;
        }
      })
      .addCase(addToCart.rejected, (state, action) => {
        state.isLoading = false;
        state.error = action.error.message || 'Failed to add to cart';
      })
      
      // updateCartItemQuantity
      .addCase(updateCartItemQuantity.pending, (state) => {
        state.isLoading = true;
        state.error = null;
      })
      .addCase(updateCartItemQuantity.fulfilled, (state, action) => {
        state.isLoading = false;
        if (action.payload) {
          state.items = action.payload.items;
          state.totalPrice = action.payload.totalPrice;
        }
      })
      .addCase(updateCartItemQuantity.rejected, (state, action) => {
        state.isLoading = false;
        state.error = action.error.message || 'Failed to update quantity';
      })
      
      // removeFromCart
      .addCase(removeFromCart.pending, (state) => {
        state.isLoading = true;
        state.error = null;
      })
      .addCase(removeFromCart.fulfilled, (state, action) => {
        state.isLoading = false;
        if (action.payload) {
          state.items = action.payload.items;
          state.totalPrice = action.payload.totalPrice;
        }
      })
      .addCase(removeFromCart.rejected, (state, action) => {
        state.isLoading = false;
        state.error = action.error.message || 'Failed to remove from cart';
      })
      
      // syncLocalCartWithServer
      .addCase(syncLocalCartWithServer.pending, (state) => {
        state.isLoading = true;
        state.error = null;
      })
      .addCase(syncLocalCartWithServer.fulfilled, (state) => {
        state.isLoading = false;
      })
      .addCase(syncLocalCartWithServer.rejected, (state, action) => {
        state.isLoading = false;
        state.error = action.error.message || 'Failed to sync cart';
      });
  },
});

export const { setAuthenticated, clearCart, initializeCart } = cartSlice.actions;
export default cartSlice.reducer; 