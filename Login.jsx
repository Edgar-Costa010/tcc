import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import api from '../api/api';

export default function Login() {
  const [email, setEmail] = useState('');
  const [senha, setSenha] = useState('');
  const [erro, setErro] = useState('');
  const navigate = useNavigate();

  const handleLogin = async (e) => {
    e.preventDefault();
    try {
      const res = await api.post('/Auth/login', { email, senha });

      const token = res.data.token;
      const nome = res.data.nome || res.data.usuario?.nome || '';
      const perfil = res.data.perfil || res.data.usuario?.perfil || '';

      localStorage.setItem('token', token);
      localStorage.setItem('nome', nome);
      localStorage.setItem('perfil', perfil);

      if (perfil === 'Administrador') navigate('/admin');
      else if (perfil === 'Profissional') navigate('/profissional');
      else navigate('/paciente');

    } catch (err) {
      setErro('Email ou senha incorretos');
    }
  };

  return (
    <div className="login-wrap">
      <div className="login-box">
        <h2 style={{ textAlign: 'center', marginBottom: 12 }}>VidaPlus - Login</h2>

        <form onSubmit={handleLogin}>
          <input
            placeholder="Email"
            type="email"
            value={email}
            onChange={e => setEmail(e.target.value)}
            required
          />

          <input
            placeholder="Senha"
            type="password"
            value={senha}
            onChange={e => setSenha(e.target.value)}
            required
          />

          <button className="btn btn-primary" style={{ width: '100%', marginTop: 8 }}>
            Entrar
          </button>

          {/* ✅ Botão para cadastro */}
          <button
            type="button"
            className="btn btn-secondary"
            style={{ width: '100%', marginTop: 10 }}
            onClick={() => navigate('/cadastro')}
          >
            Criar Conta
          </button>

          {erro && <p className="notice">{erro}</p>}
        </form>
      </div>
    </div>
  );
}
