import { useEffect, useState } from "react";
import type { ItemDto } from "../types";

export default function MyItems() {
  const [serverItems, setServerItems] = useState<ItemDto[]>([]);
  const [isLoading, setIsLoading] = useState(true);
  const [statusMessage, setStatusMessage] = useState("");

  const [newItemTitle, setNewItemTitle] = useState("");
  const [newItemDescription, setNewItemDescription] = useState("");
  const [newItemCategory, setNewItemCategory] = useState("Elektronika");
  const [newItemExchangeType, setNewItemExchangeType] = useState("Gyvai");
  const [newItemLookingFor, setNewItemLookingFor] = useState("");

  const loadItems = () => {
    setIsLoading(true);
    fetch("http://localhost:5000/api/items")
      .then((res) => res.json())
      .then((data) => {
        setServerItems(data);
        setIsLoading(false);
      })
      .catch(() => {
        setStatusMessage("Nepavyko prisijungti prie API.");
        setIsLoading(false);
      });
  };

  useEffect(() => {
    loadItems();
  }, []);

  const createItem = async () => {
    if (!newItemTitle.trim() || !newItemLookingFor.trim()) {
      setStatusMessage("Nurodykite pavadinimą ir į ką norite išsimainyti!");
      return;
    }

    const payload = {
      title: newItemTitle,
      description: newItemDescription,
      category: newItemCategory,
      exchangeType: newItemExchangeType,
      lookingFor: newItemLookingFor,
    };

    const res = await fetch("http://localhost:5000/api/items", {
      method: "POST",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify(payload),
    });

    if (res.ok) {
      setStatusMessage("Mainų skelbimas sėkmingai paskelbtas!");
      setNewItemTitle("");
      setNewItemDescription("");
      setNewItemLookingFor("");
      loadItems();
    }
  };

  const deleteItem = async (id: number) => {
    const res = await fetch(`http://localhost:5000/api/items/${id}`, { method: "DELETE" });
    if (res.ok) {
      setStatusMessage("Skelbimas pašalintas!");
      loadItems();
    }
  };

  return (
    <div className="container mt-4">
      <h3>Mano siūlomi daiktai mainams</h3>
      <div className="row mt-4">
        {/* Forma */}
        <div className="col-md-5 mb-4">
          <div className="card p-3 shadow-sm">
            <h5>+ Siūlyti naują daiktą</h5>
            <hr />
            {statusMessage && <div className="alert alert-info py-2">{statusMessage}</div>}
            <div className="mb-2">
              <label className="form-label">Daikto pavadinimas:</label>
              <input type="text" className="form-control" value={newItemTitle} onChange={(e) => setNewItemTitle(e.target.value)} placeholder="Pvz. Žieminė striukė" />
            </div>
            <div className="mb-2">
              <label className="form-label">Kategorija:</label>
              <select className="form-select" value={newItemCategory} onChange={(e) => setNewItemCategory(e.target.value)}>
                <option value="Elektronika">Elektronika</option>
                <option value="Drabužiai">Drabužiai</option>
                <option value="Transportas">Transportas</option>
                <option value="Knygos">Knygos</option>
                <option value="Buitis">Buitis</option>
              </select>
            </div>
            <div className="mb-2">
              <label className="form-label">Mainų būdas:</label>
              <select className="form-select" value={newItemExchangeType} onChange={(e) => setNewItemExchangeType(e.target.value)}>
                <option value="Gyvai">Gyvai</option>
                <option value="Siuntimu">Siuntimu</option>
                <option value="Gyvai arba siuntimu">Gyvai arba siuntimu</option>
              </select>
            </div>
            <div className="mb-2">
              <label className="form-label">Į ką norite išsimainyti?</label>
              <input type="text" className="form-control" value={newItemLookingFor} onChange={(e) => setNewItemLookingFor(e.target.value)} placeholder="Pvz. Ieškau batų" />
            </div>
            <div className="mb-2">
              <label className="form-label">Aprašymas:</label>
              <textarea className="form-control" rows={2} value={newItemDescription} onChange={(e) => setNewItemDescription(e.target.value)} placeholder="Būklė, dydis..."></textarea>
            </div>
            <button className="btn btn-success w-100 mt-3" onClick={createItem}>Paskelbti mainams</button>
          </div>
        </div>

        {/* Sąrašas */}
        <div className="col-md-7">
          <div className="card p-3 shadow-sm">
            <h5>Mano pasiūlymai serveryje ({serverItems.length})</h5>
            <hr />
            {isLoading && <p className="text-muted">Kraunama iš serverio...</p>}
            {!isLoading && serverItems.length === 0 && <p className="text-muted">Šiuo metu nesate įkėlę jokių daiktų.</p>}
            <ul className="list-group">
              {serverItems.map((item) => (
                <li key={item.id} className="list-group-item d-flex justify-content-between align-items-center">
                  <div>
                    <strong>{item.title}</strong>
                    <div className="text-muted small">Būdas: {item.exchangeType} | Keičiama į: {item.lookingFor}</div>
                  </div>
                  <button className="btn btn-outline-danger btn-sm" onClick={() => deleteItem(item.id)}>Ištrinti</button>
                </li>
              ))}
            </ul>
          </div>
        </div>
      </div>
    </div>
  );
}