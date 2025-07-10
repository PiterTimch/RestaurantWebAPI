import axios from 'axios';

// Базовий URL для API
const API_BASE_URL = process.env.REACT_APP_API_URL || 'http://localhost:5000';

// Створюємо axios інстанс з базовою конфігурацією
export const apiClient = axios.create({
  baseURL: API_BASE_URL,
  headers: {
    'Content-Type': 'application/json',
  },
});

// Інтерцептор для додавання токена авторизації
apiClient.interceptors.request.use(
  (config) => {
    const token = localStorage.getItem('token');
    if (token) {
      config.headers.Authorization = `Bearer ${token}`;
    }
    return config;
  },
  (error) => {
    return Promise.reject(error);
  }
);

// Інтерцептор для обробки помилок
apiClient.interceptors.response.use(
  (response) => response,
  (error) => {
    if (error.response?.status === 401) {
      // Якщо токен недійсний, видаляємо його
      localStorage.removeItem('token');
      // Можна додати логіку для перенаправлення на сторінку логіну
    }
    return Promise.reject(error);
  }
); 