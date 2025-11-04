// src/components/ModalLeito.jsx
import React, { useEffect } from 'react';
import { useState } from 'react';
import api from '../api/api';

const overlayStyle = {
  position: 'fixed', inset: 0, background: 'rgba(0,0,0,0.4)', display: 'flex',
  alignItems: 'center', justifyContent: 'center', zIndex: 1000
};
const modalStyle = { background:'#fff', padding:20, borderRadius:8, width: 420, maxWidth:'90%' };
const headerStyle = { display:'flex', justifyContent:'space-between', alignItems:'center', marginBottom:12 };

export default function ModalLeito({ open, onClose, onCreated }) {
  const [form, setForm] = useState({ unidade:'', numero:'', tipo:'', ocupado:false });
  const [loading, setLoading] = useState(false);

  useEffect(() => {
    const onKey = (e) => { if (e.key === 'Escape') onClose(); };
    if (open) window.addEventListener('keydown', onKey);
    return () => window.removeEventListener('keydown', onKey);
  }, [open, onClose]);

  useEffect(() => {
    if (!open) setForm({ unidade:'', numero:'', tipo:'', ocupado:false });
  }, [open]);

  const submit = async (e) => {
    e.preventDefault();
    if (!form.unidade || !form.numero) {
      alert('Preencha unidade e número');
      return;
    }
    setLoading(true);
    try {
      // envio com PascalCase conforme backend esperado
      await api.post('Leitos', {
        Unidade: form.unidade,
        Numero: form.numero,
        Ocupado: !!form.ocupado,
        Tipo: form.tipo
      });
      if (onCreated) onCreated();
      setForm({ unidade:'', numero:'', tipo:'', ocupado:false });
    } catch (err) {
      console.error('Erro cadastrar leito:', err);
      alert(err?.response?.data?.mensagem || 'Erro ao cadastrar leito');
    } finally {
      setLoading(false);
      onClose();
    }
  };

  if (!open) return null;
  return (
    <div style={overlayStyle} onMouseDown={onClose}>
      <div style={modalStyle} onMouseDown={e=>e.stopPropagation()}>
        <div style={headerStyle}>
          <h3 style={{margin:0}}>Cadastrar Leito</h3>
          <button onClick={onClose} aria-label="Fechar">✕</button>
        </div>

        <form onSubmit={submit} style={{display:'grid', gap:10}}>
          <label>
            Unidade
            <input value={form.unidade} onChange={e=>setForm({...form, unidade:e.target.value})} required />
          </label>

          <label>
            Número
            <input value={form.numero} onChange={e=>setForm({...form, numero:e.target.value})} required />
          </label>

          <label>
            Tipo (opcional)
            <input value={form.tipo} onChange={e=>setForm({...form, tipo:e.target.value})} />
          </label>

          <label style={{display:'flex', alignItems:'center', gap:8}}>
            <input type="checkbox" checked={form.ocupado} onChange={e=>setForm({...form, ocupado:e.target.checked})} />
            Ocupado
          </label>

          <div style={{display:'flex', justifyContent:'flex-end', gap:8, marginTop:6}}>
            <button type="button" className="btn btn-outline" onClick={onClose} disabled={loading}>Cancelar</button>
            <button type="submit" className="btn btn-primary" disabled={loading}>{loading ? 'Enviando...' : 'Cadastrar'}</button>
          </div>
        </form>
      </div>
    </div>
  );
}
