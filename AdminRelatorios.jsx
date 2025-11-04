// src/pages/AdminRelatorios.jsx
import React, { useEffect, useState } from 'react';
import api from '../api/api';
import Layout from '../components/Layout';
import { PieChart, Pie, Cell, Tooltip, Legend, ResponsiveContainer } from 'recharts';

export default function AdminRelatorios() {
  const [pacientes, setPacientes] = useState([]);
  const [profissionais, setProfissionais] = useState([]);
  const [consultas, setConsultas] = useState([]);
  const [leitosResumo, setLeitosResumo] = useState({ totalLeitos: 0, ocupados: 0, livres: 0 });
  const [financeiroResumo, setFinanceiroResumo] = useState({ receitas: 0, despesas: 0, saldo: 0 });
  const COLORS = ['#0088FE','#00C49F','#FFBB28','#FF8042'];

  useEffect(() => {
    const carregar = async () => {
      try {
        const [rPac, rProf, rCons, rLeitos, rFin] = await Promise.all([
          api.get('Pacientes'),
          api.get('Profissionais'),
          api.get('Consultas'),
          api.get('Leitos/relatorio'),
          api.get('Financeiro/relatorio')
        ]);
        setPacientes(rPac.data || []);
        setProfissionais(rProf.data || []);
        setConsultas(rCons.data || []);
        setLeitosResumo(rLeitos.data || { totalLeitos: 0, ocupados: 0, livres: 0 });
        setFinanceiroResumo(rFin.data || { receitas: 0, despesas: 0, saldo: 0 });
      } catch (err) {
        console.error('Erro ao carregar relatórios', err);
      }
    };

    carregar();
  }, []);

  // distribuição de status de consultas
  const statusCount = consultas.reduce((acc, c) => {
    const key = c.status || 'Desconhecido';
    acc[key] = (acc[key] || 0) + 1;
    return acc;
  }, {});
  const chartData = Object.entries(statusCount).map(([name, value]) => ({ name, value }));

  return (
    <Layout>
      <h2>Relatórios Administrativos</h2>

      <div style={{ display: 'flex', gap: 12, marginTop: 12 }}>
        <div className="card" style={{ flex: 1 }}>
          <h3>Pacientes</h3>
          <p style={{ fontSize: 28, fontWeight: 700 }}>{pacientes.length}</p>
        </div>

        <div className="card" style={{ flex: 1 }}>
          <h3>Profissionais</h3>
          <p style={{ fontSize: 28, fontWeight: 700 }}>{profissionais.length}</p>
        </div>

        <div className="card" style={{ flex: 1 }}>
          <h3>Consultas</h3>
          <p style={{ fontSize: 28, fontWeight: 700 }}>{consultas.length}</p>
        </div>

        <div className="card" style={{ flex: 1 }}>
          <h3>Leitos (ocupados / total)</h3>
          <p style={{ fontSize: 20, fontWeight: 700 }}>{leitosResumo.ocupados} / {leitosResumo.totalLeitos}</p>
          <p>Livres: {leitosResumo.livres}</p>
        </div>

        <div className="card" style={{ flex: 1 }}>
          <h3>Financeiro (saldo)</h3>
          <p>Receitas: R$ {financeiroResumo.receitas?.toFixed(2)}</p>
          <p>Despesas: R$ {financeiroResumo.despesas?.toFixed(2)}</p>
          <p style={{ fontWeight: 700 }}>Saldo: R$ {financeiroResumo.saldo?.toFixed(2)}</p>
        </div>
      </div>

      <div className="card" style={{ marginTop: 16 }}>
        <h3>Distribuição de Consultas por status</h3>
        {chartData.length > 0 ? (
          <div style={{ width: '100%', height: 320 }}>
            <ResponsiveContainer>
              <PieChart>
                <Pie dataKey="value" data={chartData} outerRadius={100} label>
                  {chartData.map((entry, index) => <Cell key={index} fill={COLORS[index % COLORS.length]} />)}
                </Pie>
                <Tooltip />
                <Legend />
              </PieChart>
            </ResponsiveContainer>
          </div>
        ) : (
          <p>Nenhuma consulta registrada ainda.</p>
        )}
      </div>
    </Layout>
  );
}