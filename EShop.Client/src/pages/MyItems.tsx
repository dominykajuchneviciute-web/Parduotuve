import { useEffect, useState } from "react";
import type { ItemDto } from "../types";

export default function MyItems() {
  const [serverItems, setServerItems] = useState<ItemDto[]>([]);
  const [isLoading, setIsLoading] = useState(true);
  const [statusMessage, setStatusMessage] = useState("");

  const [name, setName] = useState("");
  const [description, setDescription] = useState("");
  const [condition, setCondition] = useState("Naudotas");
  const [size, setSize] = useState("");
  const [manufacturer, setManufacturer] = useState("");
  const [color, setColor] = useState("");

  const loadItems = () => {
    setIsLoading(true);
    fetch("http://localhost:5000/api/items")
      .then((res) => res.json())
      .then((data) => {
        setServerItems(data);
        setIsLoading(false);
      })
      .catch(() => {
        setStatusMessage("Nepavyko pasiekti API.");
        setIsLoading(false);
      });
  };

  useEffect(() => {
    loadItems();
  }, []);

  const createItem = async () => {
    if (!name.trim()) {
      setStatusMessage("Nurodykite daikto pavadinimą!");
      return;
    }

    const payload = {
      name,
      description,
      condition,
      size,
      manufacturer,
      color
    };

    const res = await fetch("http://localhost:5000/api/items", {
      method: "POST",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify(payload),
    });

    if (res.ok) {
      setStatusMessage("Daiktas sėkmingai pridėtas!");
      setName("");
      setDescription("");
      setSize("");
      setManufacturer("");
      setColor("");
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
              <label className="form-label">Pavadinimas (Name):</label>
              <input type="text" className="form-control" value={name} onChange={(e) => setName(e.target.value)} placeholder="Pvz. Dviratis" />
            </div>
            <div className="mb-2">
              <label className="form-label">Būklė (Condition):</label>
              <select className="form-select" value={condition} onChange={(e) => setCondition(e.target.value)}>
                <option value="Naujas">Naujas</option>
                <option value="Naudotas">Naudotas</option>
              </select>
            </div>
            <div className="mb-2">
              <label className="form-label">Gamintojas (Manufacturer):</label>
              <input type="text" className="form-control" value={manufacturer} onChange={(e) => setManufacturer(e.target.value)} placeholder="Pvz. Trek, Dell" />
            </div>
            <div className="mb-2">
              <label className="form-label">Dydis (Size):</label>
              <input type="text" className="form-control" value={size} onChange={(e) => setSize(e.target.value)} placeholder="Pvz. M, 42, 27 coliai" />
            </div>
            <div className="mb-2">
              <label className="form-label">Spalva (Color):</label>
              <input type="text" className="form-control" value={color} onChange={(e) => setColor(e.target.value)} placeholder="Pvz. Juoda, Raudona" />
            </div>
            <div className="mb-2">
              <label className="form-label">Aprašymas:</label>
              <textarea className="form-control" rows={2} value={description} onChange={(e) => setDescription(e.target.value)} placeholder="Trumpas aprašymas..."></textarea>
            </div>
            <button className="btn btn-success w-100 mt-3" onClick={createItem}>Paskelbti daiktą</button>
          </div>
        </div>

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
                    <strong>{item.name}</strong>
                    <div className="text-muted small">
                      {item.manufacturer && `${item.manufacturer} | `}
                      Būklė: {item.condition}
                      {item.size && ` | Dydis: ${item.size}`}
                    </div>
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