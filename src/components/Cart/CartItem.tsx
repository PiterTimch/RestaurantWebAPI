import React from 'react';
import { CartItemModel } from '../../types/cart';

interface CartItemProps {
  item: CartItemModel;
  onIncrease: (itemId: number) => void;
  onDecrease: (itemId: number) => void;
  onRemove: (itemId: number) => void;
}

export const CartItem: React.FC<CartItemProps> = ({
  item,
  onIncrease,
  onDecrease,
  onRemove,
}) => {
  return (
    <div className="cart-item">
      <div className="cart-item-image">
        <img 
          src={`/images/${item.imageName}`} 
          alt={item.name}
          onError={(e) => {
            e.currentTarget.src = '/images/default-product.jpg';
          }}
        />
      </div>
      
      <div className="cart-item-details">
        <h3 className="cart-item-name">{item.name}</h3>
        <p className="cart-item-category">{item.categoryName}</p>
        <p className="cart-item-size">Розмір: {item.sizeName}</p>
        <p className="cart-item-price">{item.price} грн</p>
      </div>
      
      <div className="cart-item-quantity">
        <button 
          className="quantity-btn"
          onClick={() => onDecrease(item.id)}
          disabled={item.quantity <= 1}
        >
          -
        </button>
        <span className="quantity-value">{item.quantity}</span>
        <button 
          className="quantity-btn"
          onClick={() => onIncrease(item.id)}
        >
          +
        </button>
      </div>
      
      <div className="cart-item-total">
        <p className="total-price">{(item.price * item.quantity).toFixed(2)} грн</p>
        <button 
          className="remove-btn"
          onClick={() => onRemove(item.id)}
        >
          Видалити
        </button>
      </div>
    </div>
  );
}; 