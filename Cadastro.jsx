import React, { useState } from "react";
import api from "../api/api";
import { useNavigate } from "react-router-dom";

export default function Cadastro() {
  const navigate = useNavigate();
  const [form, setForm] = useState({
    nome: "",
    cpf: "",
    email: "",
    telefone: "",
    endereco: "",
    dataNascimento: "",
    senha: ""
  });

  const handleChange = (e) => {
    setForm({ ...form, [e.target.name]: e.target.value });
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    try {
      await api.post("/pacientes", form);
      alert("Cadastro realizado com sucesso!");
      navigate("/"); // volta para o login
    } catch (err) {
      alert("Erro ao cadastrar. Verifique os dados.");
      console.error(err);
    }
  };

  return (
    <div className="login-wrap">
      <div className="login-box">
        <h2 style={{ textAlign: "center", marginBottom: 12 }}>Criar Conta</h2>

        <form onSubmit={handleSubmit}>
          <input name="nome" placeholder="Nome" onChange={handleChange} required />
          <input name="cpf" placeholder="CPF" onChange={handleChange} required />
          <input name="email" type="email" placeholder="Email" onChange={handleChange} required />
          <input name="telefone" placeholder="Telefone" onChange={handleChange} required />
          <input name="endereco" placeholder="Endereço" onChange={handleChange} required />
          <input name="dataNascimento" type="date" onChange={handleChange} required />
          <input name="senha" type="password" placeholder="Senha" onChange={handleChange} required />

          <button className="btn btn-primary" style={{ width: "100%", marginTop: 8 }}>
            Cadastrar
          </button>
          <button
            type="button"
            className="btn btn-secondary"
            style={{ width: "100%", marginTop: 8 }}
            onClick={() => navigate("/")}
          >
            Voltar
          </button>
        </form>
      </div>
    </div>
  );
}