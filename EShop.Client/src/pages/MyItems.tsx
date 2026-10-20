import { useEffect, useState } from "react";
import { Link } from "react-router-dom";
import type { ItemDto } from "../types";

export default function MyItems() {
  const [serverItems, setServerItems] = useState<ItemDto[]>([]);
  const [savedItems, setSavedItems] = useState<ItemDto[]>([]);
  const [activeTab, setActiveTab] = useState<"mine" | "saved">("mine");

  const [isLoading, setIsLoading] = useState(true);
  const [isLoadingSaved, setIsLoadingSaved] = useState(false);
  const [statusMessage, setStatusMessage] = useState("");

  const [name, setName] = useState("");
  const [description, setDescription] = useState("");
  const [condition, setCondition] = useState("Used");
  const [category, setCategory] = useState("Clothing");
  const [size, setSize] = useState("");
  const [manufacturer, setManufacturer] = useState("");
  const [color, setColor] = useState("");
  const [model, setModel] = useState("");
  const [material, setMaterial] = useState("");
  const [dimensions, setDimensions] = useState("");
  const [year, setYear] = useState("");
  const [shoeSize, setShoeSize] = useState("");
  const [instrumentType, setInstrumentType] = useState("");

  const loadItems = () => {
    setIsLoading(true);
    fetch("http://localhost:5145/api/items/mine", { credentials: "include" })
      .then((res) => {
        if (!res.ok) {
          throw new Error(`HTTP ${res.status}`);
        }
        return res.json();
      })
      .then((data) => {
        setServerItems(data);
        setIsLoading(false);
      })
      .catch(() => {
        setStatusMessage("Nepavyko pasiekti API.");
        setIsLoading(false);
      });
  };

  const loadSavedItems = () => {
    setIsLoadingSaved(true);
    fetch("http://localhost:5145/api/items/saved", { credentials: "include" })
      .then((res) => {
        if (!res.ok) {
          throw new Error(`HTTP ${res.status}`);
        }
        return res.json();
      })
      .then((data) => {
        setSavedItems(data);
        setIsLoadingSaved(false);
      })
      .catch((err) => {
        console.error("Klaida kraunant įsimintus daiktus:", err);
        setIsLoadingSaved(false);
      });
  };

  useEffect(() => {
    loadItems();
    loadSavedItems();
  }, []);

  const createItem = async () => {
    if (!name.trim()) {
      setStatusMessage("Nurodykite daikto pavadinimą!");
      return;
    }

    const payload = {
      category,
      name,
      description,
      condition,
      size: category === "Clothing" && size ? Number(size) : null,
      shoeSize: category === "Footwear" && shoeSize ? Number(shoeSize) : null,
      manufacturer: category !== "Furniture" ? manufacturer || null : null,
      color: ["Clothing", "Electronics", "Furniture", "Footwear"].includes(category)
        ? color || null
        : null,
      model: ["Electronics", "Transport"].includes(category) ? model || null : null,
      material: category === "Furniture" ? material || null : null,
      dimensions: category === "Furniture" ? dimensions || null : null,
      year: ["Transport", "Instrument"].includes(category) && year
        ? Number(year)
        : null,
      instrumentType: category === "Instrument" ? instrumentType || null : null
    };

    try {
      const res = await fetch("http://localhost:5145/api/items", {
        method: "POST",
        headers: {
          "Content-Type": "application/json"
        },
        credentials: "include",
        body: JSON.stringify(payload)
      });

      if (!res.ok) {
        const errorText = await res.text();
        setStatusMessage(`Klaida (${res.status}): ${errorText}`);
        return;
      }

      setStatusMessage("Daiktas sėkmingai pridėtas!");

      setName("");
      setDescription("");
      setSize("");
      setManufacturer("");
      setColor("");
      setModel("");
      setMaterial("");
      setDimensions("");
      setYear("");
      setShoeSize("");
      setInstrumentType("");

      loadItems();
    } catch (error) {
      console.error(error);
      setStatusMessage("Nepavyko pasiekti API.");
    }
  };

  const deleteItem = async (id: number) => {
    const res = await fetch(`http://localhost:5145/api/items/${id}`, {
      method: "DELETE",
      credentials: "include",
    });
    if (res.ok) {
      setStatusMessage("Skelbimas pašalintas!");
      loadItems();
    }
  };

  const removeSavedItem = async (id: number) => {
    try {
      const res = await fetch(`http://localhost:5145/api/items/${id}/saved/toggle`, {
        method: "POST",
        credentials: "include",
      });
      if (res.ok) {
        loadSavedItems();
      }
    } catch (error) {
      console.error("Klaida šalinant iš įsimintų:", error);
    }
  };

  return (
    <div className="container mt-4">
      <h3>Mano paskyra ir siūlomi daiktai</h3>
      <div className="row mt-4">
        <div className="col-md-5 mb-4">
          <div className="card p-3 shadow-sm">
            <h5>+ Siūlyti naują daiktą</h5>
            <hr />

            {statusMessage && (
              <div className="alert alert-info py-2">{statusMessage}</div>
            )}

            <div className="mb-2">
              <label className="form-label">Kategorija:</label>
              <select
                className="form-select"
                value={category}
                onChange={(e) => setCategory(e.target.value)}
              >
                <option value="Clothing">Drabužiai</option>
                <option value="Electronics">Elektronika</option>
                <option value="Furniture">Baldai</option>
                <option value="Transport">Transportas</option>
                <option value="Footwear">Avalynė</option>
                <option value="Instrument">Muzikos instrumentai</option>
              </select>
            </div>

            <div className="mb-2">
              <label className="form-label">Pavadinimas:</label>
              <input
                type="text"
                className="form-control"
                value={name}
                onChange={(e) => setName(e.target.value)}
                placeholder="Pavadinimas"
              />
            </div>

            <div className="mb-2">
              <label className="form-label">Būklė:</label>
              <select
                className="form-select"
                value={condition}
                onChange={(e) => setCondition(e.target.value)}
              >
                <option value="New">Naujas</option>
                <option value="VeryGood">Labai gera</option>
                <option value="Good">Gera</option>
                <option value="Decent">Patenkinama</option>
                <option value="Used">Naudotas</option>
                <option value="VeryUsed">Labai naudotas</option>
              </select>
            </div>

            <div className="mb-2">
              <label className="form-label">Aprašymas:</label>
              <textarea
                className="form-control"
                rows={2}
                value={description}
                onChange={(e) => setDescription(e.target.value)}
                placeholder="Trumpas aprašymas..."
              />
            </div>

            {category !== "Furniture" && (
              <div className="mb-2">
                <label className="form-label">Gamintojas:</label>
                <input
                  type="text"
                  className="form-control"
                  value={manufacturer}
                  onChange={(e) => setManufacturer(e.target.value)}
                  placeholder="Gamintojas"
                />
              </div>
            )}

            {category === "Clothing" && (
              <div className="mb-2">
                <label className="form-label">Drabužio dydis:</label>
                <input
                  type="number"
                  className="form-control"
                  value={size}
                  onChange={(e) => setSize(e.target.value)}
                  placeholder="Pvz. 42"
                />
              </div>
            )}

            {category === "Footwear" && (
              <div className="mb-2">
                <label className="form-label">Batų dydis:</label>
                <input
                  type="number"
                  className="form-control"
                  value={shoeSize}
                  onChange={(e) => setShoeSize(e.target.value)}
                  placeholder="Pvz. 38"
                />
              </div>
            )}

            {["Clothing", "Electronics", "Furniture", "Footwear"].includes(category) && (
              <div className="mb-2">
                <label className="form-label">Spalva:</label>
                <input
                  type="text"
                  className="form-control"
                  value={color}
                  onChange={(e) => setColor(e.target.value)}
                  placeholder="Pvz. Juoda, raudona"
                />
              </div>
            )}

            {["Electronics", "Transport"].includes(category) && (
              <div className="mb-2">
                <label className="form-label">Modelis:</label>
                <input
                  type="text"
                  className="form-control"
                  value={model}
                  onChange={(e) => setModel(e.target.value)}
                  placeholder="Daikto modelis"
                />
              </div>
            )}

            {category === "Furniture" && (
              <>
                <div className="mb-2">
                  <label className="form-label">Medžiaga:</label>
                  <input
                    type="text"
                    className="form-control"
                    value={material}
                    onChange={(e) => setMaterial(e.target.value)}
                    placeholder="Pvz. Medis, metalas"
                  />
                </div>
                <div className="mb-2">
                  <label className="form-label">Matmenys:</label>
                  <input
                    type="text"
                    className="form-control"
                    value={dimensions}
                    onChange={(e) => setDimensions(e.target.value)}
                    placeholder="Pvz. 120 × 60 × 75 cm"
                  />
                </div>
              </>
            )}

            {["Transport", "Instrument"].includes(category) && (
              <div className="mb-2">
                <label className="form-label">Pagaminimo metai:</label>
                <input
                  type="number"
                  className="form-control"
                  value={year}
                  onChange={(e) => setYear(e.target.value)}
                  placeholder="Pvz. 2022"
                />
              </div>
            )}

            {category === "Instrument" && (
              <div className="mb-2">
                <label className="form-label">Instrumento tipas:</label>
                <input
                  type="text"
                  className="form-control"
                  value={instrumentType}
                  onChange={(e) => setInstrumentType(e.target.value)}
                  placeholder="Pvz. Gitara, pianinas"
                />
              </div>
            )}

            <button
              className="btn btn-success w-100 mt-3"
              onClick={createItem}
            >
              Paskelbti daiktą
            </button>
          </div>
        </div>

        <div className="col-md-7">
          <div className="card p-3 shadow-sm">
            <ul className="nav nav-pills mb-3">
              <li className="nav-item">
                <button
                  type="button"
                  className={`nav-link ${activeTab === "mine" ? "active" : ""}`}
                  onClick={() => setActiveTab("mine")}
                >
                  Mano pasiūlymai ({serverItems.length})
                </button>
              </li>
              <li className="nav-item ms-2">
                <button
                  type="button"
                  className={`nav-link ${activeTab === "saved" ? "active bg-danger" : "text-danger"}`}
                  onClick={() => setActiveTab("saved")}
                >
                  Įsiminti daiktai ({savedItems.length})
                </button>
              </li>
            </ul>

            <hr className="mt-0" />

            {activeTab === "mine" && (
              <>
                {isLoading && <p className="text-muted">Kraunama iš serverio...</p>}
                {!isLoading && serverItems.length === 0 && (
                  <p className="text-muted">Šiuo metu nesate įkėlę jokių daiktų.</p>
                )}
                <ul className="list-group">
                  {serverItems.map((item) => (
                    <li
                      key={item.id}
                      className="list-group-item d-flex justify-content-between align-items-center"
                    >
                      <div>
                        <strong>{item.name}</strong>
                        <div className="text-muted small">
                          {item.manufacturer && `${item.manufacturer} | `}
                          Būklė: {item.condition}
                          {item.size && ` | Dydis: ${item.size}`}
                        </div>
                      </div>
                      <div className="d-flex gap-2">
                        <Link to={`/item/${item.id}`} className="btn btn-outline-primary btn-sm">
                          Peržiūrėti
                        </Link>
                        <button
                          className="btn btn-outline-danger btn-sm"
                          onClick={() => deleteItem(item.id)}
                        >
                          Ištrinti
                        </button>
                      </div>
                    </li>
                  ))}
                </ul>
              </>
            )}

            {activeTab === "saved" && (
              <>
                {isLoadingSaved && <p className="text-muted">Kraunami įsiminti daiktai...</p>}
                {!isLoadingSaved && savedItems.length === 0 && (
                  <p className="text-muted">Neturite įsimintų daiktų. Spauskite „Įsiminti“ prie bet kurio skelbimo!</p>
                )}
                <ul className="list-group">
                  {savedItems.map((item) => (
                    <li
                      key={item.id}
                      className="list-group-item d-flex justify-content-between align-items-center"
                    >
                      <div>
                        <strong>{item.name}</strong>
                        <div className="text-muted small">
                          {item.manufacturer && `${item.manufacturer} | `}
                          Būklė: {item.condition}
                          {item.size && ` | Dydis: ${item.size}`}
                        </div>
                      </div>
                      <div className="d-flex gap-2">
                        <Link to={`/item/${item.id}`} className="btn btn-outline-primary btn-sm">
                          Atidaryti
                        </Link>
                        <button
                          className="btn btn-outline-secondary btn-sm"
                          onClick={() => removeSavedItem(item.id)}
                          title="Pašalinti iš įsimintų"
                        >
                          ✕ Pašalinti
                        </button>
                      </div>
                    </li>
                  ))}
                </ul>
              </>
            )}
          </div>
        </div>
      </div>
    </div>
  );
}