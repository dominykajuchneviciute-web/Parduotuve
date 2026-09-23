import { Link } from "react-router-dom";

export default function Home() {
  return (
    <div className="container mt-4">
      <div className="p-5 mb-4 bg-light rounded-3 text-center border shadow-sm">
        <h1 className="display-5 fw-bold">Daiktų mainų platforma</h1>
        <p className="fs-5 text-muted">Apsikeiskite nereikalingais daiktais be jokių pinigų – gyvai arba siuntimu!</p>
        <div className="d-flex justify-content-center gap-3 mt-3">
          <Link to="/items" className="btn btn-primary btn-lg">Naršyti daiktus</Link>
          <Link to="/my-items" className="btn btn-outline-secondary btn-lg">Pasiūlyti mainams</Link>
        </div>
      </div>

      <h3 className="mt-5 mb-3">Naujausi mainų pasiūlymai</h3>
      <div className="row">
        <div className="col-md-6 mb-3">
          <div className="card h-100 shadow-sm border-start border-4">
            <div className="card-body">
              <h5>Miesto dviratis</h5>
              <p className="text-muted small mb-2">Maino būdas: <span className="badge bg-info text-dark">Gyvai</span></p>
              <p className="mb-3"><strong>Ieško mainams:</strong> Paspirtuko arba laikrodžio</p>
              <Link to="/item/1" className="btn btn-sm btn-outline-primary">Peržiūrėti daiktą</Link>
            </div>
          </div>
        </div>

        <div className="col-md-6 mb-3">
          <div className="card h-100 shadow-sm border-start border-4">
            <div className="card-body">
              <h5>Dell 24" monitorius</h5>
              <p className="text-muted small mb-2">Maino būdas: <span className="badge bg-secondary">Siuntimu</span></p>
              <p className="mb-3"><strong>Ieško mainams:</strong> Grafinės planšetės</p>
              <Link to="/item/2" className="btn btn-sm btn-outline-primary">Peržiūrėti daiktą</Link>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
}