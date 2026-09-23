export default function Login() {
  return (
    <div className="container mt-5" style={{ maxWidth: "420px" }}>
      <div className="card p-4 shadow-sm">
        <h3 className="text-center mb-3">Prisijungimas</h3>
        <div className="mb-3">
          <label className="form-label">El. paštas:</label>
          <input type="email" className="form-control" placeholder="vardas@pastas.lt" />
        </div>
        <div className="mb-3">
          <label className="form-label">Slaptažodis:</label>
          <input type="password" className="form-control" placeholder="••••••••" />
        </div>
        <button className="btn btn-primary w-100">Prisijungti</button>
        <div className="text-center mt-3">
          <small className="text-muted">Neturite paskyros? <a href="#">Registruotis</a></small>
        </div>
      </div>
    </div>
  );
}