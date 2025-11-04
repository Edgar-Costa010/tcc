import React from 'react';
import { Navigate } from 'react-router-dom';
export default function ProtectedRoute({ children, role }) {
  const token = localStorage.getItem('token');
  const perfil = localStorage.getItem('perfil');
  if (!token) return <Navigate to="/" />;
  if (role && perfil !== role) return <Navigate to="/" />;
  return children;
}
