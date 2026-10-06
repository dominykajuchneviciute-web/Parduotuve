import { useState } from "react";
import type { FormEvent } from "react";
import { Link, useNavigate } from "react-router-dom";
import { useAuth } from "../AuthContext";

export default function Register() {
  const { register } = useAuth();
  const navigate = useNavigate();
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [confirmPassword, setConfirmPassword] = useState("");
  const [error, setError] = useState<string | null>(null);
  const [submitting, setSubmitting] = useState(false);

  async function handleSubmit(e: FormEvent) {
    e.preventDefault();
    setError(null);

    if (password !== confirmPassword) {
      setError("Slaptažodžiai nesutampa.");
      return;
    }

    setSubmitting(true);
    const errorMessage = await register(email, password);
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
        <h3 className="text-center mb-3">Registracija</h3>
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
          <div className="form-text">
            Bent 6 simboliai, didžioji ir mažoji raidė, skaitmuo ir simbolis.
          </div>
        </div>
        <div className="mb-3">
          <label className="form-label">Pakartokite slaptažodį:</label>
          <input
            type="password"
            className="form-control"
            placeholder="••••••••"
            value={confirmPassword}
            onChange={(e) => setConfirmPassword(e.target.value)}
            required
          />
        </div>
        <button className="btn btn-primary w-100" disabled={submitting}>
          Registruotis
        </button>
        <div className="text-center mt-3">
          <small className="text-muted">
            Jau turite paskyrą? <Link to="/login">Prisijungti</Link>
          </small>
        </div>
      </form>
    </div>
  );
}