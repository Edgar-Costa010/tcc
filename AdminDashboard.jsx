// src/pages/AdminDashboard.jsx
import React, { useEffect, useState } from 'react';
import api from '../api/api';
import Layout from '../components/Layout';
import ModalLeito from '../components/ModalLeito';
import ModalFinanceiro from '../components/ModalFinanceiro';

export default function AdminDashboard() {
  const [pacientes, setPacientes] = useState([]);
  const [profissionais, setProfissionais] = useState([]);

  const [novoAdmin, setNovoAdmin] = useState({ nome:'', email:'', cpf:'', telefone:'', senha:'' });
  const [novoProf, setNovoProf] = useState({ nome:'', cpf:'', crm:'', especialidade:'', telefone:'', email:'', unidade:'', senha:'' });

  const [loading, setLoading] = useState(false);

  // 🔹 estados dos modais
  const [showModalLeito, setShowModalLeito] = useState(false);
  const [showModalFin, setShowModalFin] = useState(false);

  const carregar = async () => {
    try {
      const [r1, r2] = await Promise.all([
        api.get('Pacientes'),
        api.get('Profissionais')
      ]);
      setPacientes(r1.data || []);
      setProfissionais(r2.data || []);
    } catch (err) {
      console.error("Erro ao carregar dados:", err);
    }
  };

  useEffect(() => { carregar(); }, []);

  const cadastrarAdmin = async (e) => {
    e.preventDefault();
    setLoading(true);
    try {
      await api.post('auth/register', { ...novoAdmin, perfil: 'Administrador' });
      setNovoAdmin({ nome:'', email:'', cpf:'', telefone:'', senha:'' });
      alert('Administrador cadastrado');
      carregar();
    } catch (err) {
      console.error(err);
      alert('Erro ao cadastrar administrador');
    } finally {
      setLoading(false);
    }
  };

  const cadastrarProf = async (e) => {
    e.preventDefault();
    setLoading(true);
    try {
      await api.post('Profissionais', { ...novoProf });
      setNovoProf({ nome:'', cpf:'', crm:'', especialidade:'', telefone:'', email:'', unidade:'', senha:'' });
      alert('Profissional cadastrado');
      carregar();
    } catch (err) {
      console.error(err);
      alert('Erro ao cadastrar profissional');
    } finally {
      setLoading(false);
    }
  };

  return (
    <Layout>
      <h2>Painel do Administrador</h2>

      <div style={{ display: 'flex', gap: 20 }}>
        {/* ---------- ADMIN ---------- */}
        <div className="card" style={{ flex: 1 }}>
          <h3>Cadastrar Administrador</h3>
          <form className="form" onSubmit={cadastrarAdmin}>
            <input placeholder="Nome" value={novoAdmin.nome} onChange={e=>setNovoAdmin({...novoAdmin,nome:e.target.value})} required />
            <input placeholder="Email" type="email" value={novoAdmin.email} onChange={e=>setNovoAdmin({...novoAdmin,email:e.target.value})} required />
            <input placeholder="CPF" value={novoAdmin.cpf} onChange={e=>setNovoAdmin({...novoAdmin,cpf:e.target.value})} required />
            <input placeholder="Telefone" value={novoAdmin.telefone} onChange={e=>setNovoAdmin({...novoAdmin,telefone:e.target.value})} />
            <input placeholder="Senha" type="password" value={novoAdmin.senha} onChange={e=>setNovoAdmin({...novoAdmin,senha:e.target.value})} required />
            <button className="btn btn-primary" type="submit" disabled={loading}>
              {loading ? 'Enviando...' : 'Cadastrar Administrador'}
            </button>
          </form>
        </div>

        {/* ---------- PROFISSIONAL ---------- */}
        <div className="card" style={{ flex: 1 }}>
          <h3>Cadastrar Profissional</h3>
          <form className="form" onSubmit={cadastrarProf}>
            <input placeholder="Nome" value={novoProf.nome} onChange={e=>setNovoProf({...novoProf,nome:e.target.value})} required />
            <input placeholder="CPF" value={novoProf.cpf} onChange={e=>setNovoProf({...novoProf,cpf:e.target.value})} required />
            <input placeholder="CRM" value={novoProf.crm} onChange={e=>setNovoProf({...novoProf,crm:e.target.value})} required />
            <input placeholder="Especialidade" value={novoProf.especialidade} onChange={e=>setNovoProf({...novoProf,especialidade:e.target.value})} required />
            <input placeholder="Telefone" value={novoProf.telefone} onChange={e=>setNovoProf({...novoProf,telefone:e.target.value})} required />
            <input placeholder="E-mail" value={novoProf.email} onChange={e=>setNovoProf({...novoProf,email:e.target.value})} />
            <input placeholder="Unidade" value={novoProf.unidade} onChange={e=>setNovoProf({...novoProf,unidade:e.target.value})} />
            <input placeholder="Senha" type="password" value={novoProf.senha} onChange={e=>setNovoProf({...novoProf,senha:e.target.value})} />
            <button className="btn btn-green" type="submit" disabled={loading}>
              {loading ? 'Enviando...' : 'Cadastrar Profissional'}
            </button>
          </form>
        </div>
      </div>

      {/* ✅ BOTÕES EXTRA */}
      <div style={{ marginTop: 15, display: 'flex', gap: 12 }}>
        <button className="btn btn-outline" onClick={() => setShowModalLeito(true)}>
          + Cadastrar Leitos
        </button>

        <button className="btn btn-outline" onClick={() => setShowModalFin(true)}>
          + Registrar Lançamento Financeiro
        </button>
      </div>

      {/* ---------- LISTAGEM PACIENTES ---------- */}
      <div className="card" style={{ marginTop: 20 }}>
        <h3>Pacientes</h3>
        <table className="table">
          <thead><tr><th>Nome</th><th>Email</th><th>CPF</th><th>Telefone</th></tr></thead>
          <tbody>{pacientes.map(p=>(
            <tr key={p.id}><td>{p.nome}</td><td>{p.email}</td><td>{p.cpf}</td><td>{p.telefone}</td></tr>
          ))}</tbody>
        </table>
      </div>

      {/* ---------- LISTAGEM PROFISSIONAIS ---------- */}
      <div className="card" style={{ marginTop: 20 }}>
        <h3>Profissionais</h3>
        <table className="table">
          <thead><tr><th>Nome</th><th>Email</th><th>CRM</th><th>Especialidade</th><th>Unidade</th></tr></thead>
          <tbody>{profissionais.map(p=>(
            <tr key={p.id}><td>{p.nome}</td><td>{p.email}</td><td>{p.crm}</td><td>{p.especialidade}</td><td>{p.unidade}</td></tr>
          ))}</tbody>
        </table>
      </div>

      {/* ✅ MODAIS */}
      <ModalLeito open={showModalLeito} onClose={() => setShowModalLeito(false)} onCreated={() => alert("Leito cadastrado")} />
      <ModalFinanceiro open={showModalFin} onClose={() => setShowModalFin(false)} onCreated={() => alert("Financeiro registrado")} />
    </Layout>
  );
}
