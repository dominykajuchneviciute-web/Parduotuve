import { useState } from "react";
import type { FormEvent } from "react";
import { useNavigate } from "react-router-dom";
import { useAuth } from "../AuthContext";

export default function Login() {
  const { login } = useAuth();
  const navigate = useNavigate();
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [error, setError] = useState<string | null>(null);
  const [submitting, setSubmitting] = useState(false);

  async function handleSubmit(e: FormEvent) {
    e.preventDefault();
    setError(null);
    setSubmitting(true);
    const errorMessage = await login(email, password);
    setSubmitting(false);
    if (errorMessage) {
      setError(errorMessage);
    } else {
      navigate("/");
    }
  }

  return (
    <div className="container mt-5" style={{ maxWidth: "420px" }}>
      <form className="card p-4 shadow-sm" onSubmit={handleSubmit}>
        <h3 className="text-center mb-3">Prisijungimas</h3>
        {error && <div className="alert alert-danger">{error}</div>}
        <div className="mb-3">
          <label className="form-label">El. paštas:</label>
          <input
            type="email"
            className="form-control"
            placeholder="vardas@pastas.lt"
            value={email}
            onChange={(e) => setEmail(e.target.value)}
            required
          />
        </div>
        <div className="mb-3">
          <label className="form-label">Slaptažodis:</label>
          <input
            type="password"
            className="form-control"
            placeholder="••••••••"
            value={password}
            onChange={(e) => setPassword(e.target.value)}
            required
          />
        </div>
        <button className="btn btn-primary w-100" disabled={submitting}>
          Prisijungti
        </button>
        <div className="text-center mt-3">
          <small className="text-muted">
            Neturite paskyros? <a href="#">Registruotis</a>
          </small>
        </div>
      </form>
    </div>
  );
}