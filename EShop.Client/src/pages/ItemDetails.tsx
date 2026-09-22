import { useEffect, useState } from "react";
import { useParams, Link } from "react-router-dom";
import type { ItemDto } from "../types";

export default function ItemDetails() {
  const { id } = useParams<{ id: string }>();
  const [item, setItem] = useState<ItemDto | null>(null);
  const [isLoading, setIsLoading] = useState(true);
  const [showProposalAlert, setShowProposalAlert] = useState(false);

  useEffect(() => {
    fetch(`http://localhost:5000/api/items/${id}`)
      .then((res) => {
        if (!res.ok) throw new Error("Not found");
        return res.json();
      })
      .then((data) => {
        setItem(data);
        setIsLoading(false);
      })
      .catch(() => {
        setItem(null);
        setIsLoading(false);
      });
  }, [id]);

  return (
    <div className="container mt-4">
      <Link to="/items" className="btn btn-secondary btn-sm mb-3">← Atgal į daiktų sąrašą</Link>

      {isLoading && <div className="alert alert-info">Kraunama informacija apie daiktą...</div>}
      {!isLoading && !item && <div className="alert alert-warning">Daiktas su ID: {id} nerastas.</div>}

      {item && (
        <div className="card p-4 shadow-sm">
          <div className="row">
            <div className="col-md-6 d-flex align-items-center justify-content-center bg-light" style={{ minHeight: "280px", borderRadius: "8px" }}>
              <span className="fs-4 text-muted">Daikto nuotrauka</span>
            </div>
            <div className="col-md-6">
              <h2>{item.title}</h2>
              <div className="my-3">
                <span className="badge bg-secondary fs-6 me-2">{item.category}</span>
                <span className="badge bg-info text-dark fs-6">Mainai: {item.exchangeType}</span>
              </div>

              <div className="p-3 my-3 bg-light border rounded">
                <h6 className="text-primary fw-bold mb-1">Savininkas norėtų išsikeisti į:</h6>
                <p className="mb-0">{item.lookingFor}</p>
              </div>

              <p><strong>Aprašymas:</strong> {item.description}</p>
              <hr />
              <button className="btn btn-success me-2" onClick={() => setShowProposalAlert(true)}>🤝 Siūlyti mainus</button>
              <button className="btn btn-outline-danger">Įsiminti</button>

              {showProposalAlert && (
                <div className="alert alert-success mt-3 py-2">
                  Pasiūlymas išsiųstas! Savininkas su jumis susisieks.
                </div>
              )}
            </div>
          </div>
        </div>
      )}
    </div>
  );
}