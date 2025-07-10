import React, { useState } from 'react';
import { useCart } from '../../hooks/useCart';

interface AddToCartButtonProps {
  product: {
    id: number;
    name: string;
    price: number;
    imageName: string;
    categoryName: string;
    sizeName: string;
  };
  className?: string;
}

export const AddToCartButton: React.FC<AddToCartButtonProps> = ({
  product,
  className = '',
}) => {
  const { addToCart, cart } = useCart();
  const [isAdding, setIsAdding] = useState(false);

  // Перевіряємо чи товар вже є в корзині
  const existingItem = cart.items.find(item => item.productId === product.id);
  const currentQuantity = existingItem?.quantity || 0;

  const handleAddToCart = async () => {
    setIsAdding(true);
    try {
      await addToCart(product);
    } catch (error) {
      console.error('Помилка додавання в корзину:', error);
    } finally {
      setIsAdding(false);
    }
  };

  return (
    <div className={`add-to-cart-container ${className}`}>
      {currentQuantity > 0 && (
        <div className="quantity-indicator">
          В корзині: {currentQuantity}
        </div>
      )}
      
      <button
        className={`add-to-cart-btn ${isAdding ? 'loading' : ''}`}
        onClick={handleAddToCart}
        disabled={isAdding}
      >
        {isAdding ? 'Додавання...' : 'Додати в корзину'}
      </button>
    </div>
  );
}; 