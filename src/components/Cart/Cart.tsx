import React from 'react';
import { useCart } from '../../hooks/useCart';
import { CartItem } from './CartItem';

export const Cart: React.FC = () => {
  const {
    cart,
    increaseQuantity,
    decreaseQuantity,
    removeFromCart,
  } = useCart();

  if (cart.isLoading) {
    return (
      <div className="cart-container">
        <div className="loading">Завантаження корзини...</div>
      </div>
    );
  }

  if (cart.error) {
    return (
      <div className="cart-container">
        <div className="error">Помилка: {cart.error}</div>
      </div>
    );
  }

  if (cart.items.length === 0) {
    return (
      <div className="cart-container">
        <div className="empty-cart">
          <h2>Корзина порожня</h2>
          <p>Додайте товари до корзини для оформлення замовлення</p>
        </div>
      </div>
    );
  }

  return (
    <div className="cart-container">
      <h1 className="cart-title">Корзина</h1>
      
      <div className="cart-items">
        {cart.items.map((item) => (
          <CartItem
            key={item.id}
            item={item}
            onIncrease={increaseQuantity}
            onDecrease={decreaseQuantity}
            onRemove={removeFromCart}
          />
        ))}
      </div>
      
      <div className="cart-summary">
        <div className="cart-total">
          <h3>Загальна сума: {cart.totalPrice.toFixed(2)} грн</h3>
        </div>
        
        <div className="cart-actions">
          <button className="btn btn-secondary">
            Продовжити покупки
          </button>
          <button className="btn btn-primary">
            Оформити замовлення
          </button>
        </div>
      </div>
    </div>
  );
}; 