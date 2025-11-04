// src/components/ModalFinanceiro.jsx
import React, { useEffect } from 'react';
import { useState } from 'react';
import api from '../api/api';

const overlayStyle = {
  position: 'fixed', inset: 0, background: 'rgba(0,0,0,0.4)', display: 'flex',
  alignItems: 'center', justifyContent: 'center', zIndex: 1000
};
const modalStyle = { background:'#fff', padding:20, borderRadius:8, width: 480, maxWidth:'94%' };

export default function ModalFinanceiro({ open, onClose, onCreated }) {
  const [form, setForm] = useState({ tipo:'Entrada', descricao:'', valor:'', data:'' });
  const [loading, setLoading] = useState(false);

  useEffect(() => {
    const onKey = (e) => { if (e.key === 'Escape') onClose(); };
    if (open) window.addEventListener('keydown', onKey);
    return () => window.removeEventListener('keydown', onKey);
  }, [open, onClose]);

  useEffect(() => {
    if (!open) setForm({ tipo:'Entrada', descricao:'', valor:'', data:'' });
  }, [open]);

  const submit = async (e) => {
    e.preventDefault();
    const valorNum = parseFloat((form.valor || '').toString().replace(',', '.'));
    if (isNaN(valorNum) || valorNum <= 0) {
      alert('Informe um valor válido maior que 0');
      return;
    }
    setLoading(true);
    try {
      // Data: se preenchida usamos a data escolhida, senão usamos agora
      const dataIso = form.data ? new Date(form.data).toISOString() : new Date().toISOString();
      await api.post('Financeiro', {
        Data: dataIso,
        Valor: valorNum,
        Tipo: form.tipo,
        Descricao: form.descricao,
        Unidade: null
      });
      if (onCreated) onCreated();
      setForm({ tipo:'Entrada', descricao:'', valor:'', data:'' });
    } catch (err) {
      console.error('Erro cadastrar financeiro:', err);
      alert(err?.response?.data?.mensagem || 'Erro ao cadastrar lançamento');
    } finally {
      setLoading(false);
      onClose();
    }
  };

  if (!open) return null;
  return (
    <div style={overlayStyle} onMouseDown={onClose}>
      <div style={modalStyle} onMouseDown={e=>e.stopPropagation()}>
        <div style={{display:'flex', justifyContent:'space-between', alignItems:'center', marginBottom:12}}>
          <h3 style={{margin:0}}>Registrar Lançamento Financeiro</h3>
          <button onClick={onClose} aria-label="Fechar">✕</button>
        </div>

        <form onSubmit={submit} style={{display:'grid', gap:10}}>
          <label>
            Tipo
            <select value={form.tipo} onChange={e=>setForm({...form, tipo:e.target.value})}>
              <option value="Entrada">Entrada</option>
              <option value="Saida">Saída</option>
            </select>
          </label>

          <label>
            Valor (ex: 350.00)
            <input value={form.valor} onChange={e=>setForm({...form, valor: e.target.value})} placeholder="0.00" required />
          </label>

          <label>
            Descrição
            <input value={form.descricao} onChange={e=>setForm({...form, descricao: e.target.value})} />
          </label>

          <label>
            Data (opcional)
            <input type="date" value={form.data} onChange={e=>setForm({...form, data: e.target.value})} />
            <small style={{color:'#666'}}>Se vazio, usará a data/hora atual</small>
          </label>

          <div style={{display:'flex', justifyContent:'flex-end', gap:8, marginTop:6}}>
            <button type="button" className="btn btn-outline" onClick={onClose} disabled={loading}>Cancelar</button>
            <button type="submit" className="btn btn-primary" disabled={loading}>{loading ? 'Enviando...' : 'Registrar'}</button>
          </div>
        </form>
      </div>
    </div>
  );
}
