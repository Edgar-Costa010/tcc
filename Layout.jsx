import React from 'react';
import { useNavigate } from 'react-router-dom';
export default function Layout({ children }) {
  const navigate = useNavigate();
  const nome = localStorage.getItem('nome') || 'Usuário';
  const perfil = localStorage.getItem('perfil') || '';
  const logout = () => { localStorage.clear(); navigate('/'); };
  const menu = { Administrador: [ { label: 'Dashboard', path: '/admin' }, { label: 'Relatórios', path: '/admin/relatorios' } ], Medico: [ { label: 'Minhas Consultas', path: '/medico' } ], Paciente: [ { label: 'Minhas Consultas', path: '/paciente' } ] };
  return (
    <div className="container">
      <aside className="sidebar">
        <h2>VidaPlus</h2>
        {menu[perfil] && menu[perfil].map(item => (<button key={item.path} onClick={() => navigate(item.path)} style={{display:'block', width:'100%', textAlign:'left', padding:8, marginBottom:6, background:'transparent', color:'#fff', border:'none'}}>{item.label}</button>))}
      </aside>
      <div className="content">
        <div className="header">
          <div><strong>Painel {perfil}</strong></div>
          <div>
            <span style={{marginRight:12}}>👤 {nome}</span>
            <button onClick={logout} className="btn">Sair</button>
          </div>
        </div>
        <div className="main">{children}</div>
      </div>
    </div>
  );
}
