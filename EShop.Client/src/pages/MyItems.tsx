import { useEffect, useState } from "react";
import type { ItemDto } from "../types";

export default function MyItems() {
  const [serverItems, setServerItems] = useState<ItemDto[]>([]);
  const [isLoading, setIsLoading] = useState(true);
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
    fetch("http://localhost:5145/api/items")
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
                body: JSON.stringify(payload)
            });

            if (!res.ok) {
                const errorText = await res.text();

                setStatusMessage(
                    `Klaida (${res.status}): ${errorText}`
                );

                console.log("POST klaida:", res.status, errorText);
                return;
            }

            setStatusMessage("Daiktas sėkmingai pridėtas!");

            setName("");
            setDescription("");
            setSize("");
            setManufacturer("");
            setColor("");

            loadItems();
        } catch (error) {
            console.error(error);
            setStatusMessage("Nepavyko pasiekti API.");
        }
    };

  const deleteItem = async (id: number) => {
    const res = await fetch(`http://localhost:5145/api/items/${id}`, { method: "DELETE" });
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

            {statusMessage && (
              <div className="alert alert-info py-2">{statusMessage}</div>
            )}

            {/* Kategorija */}
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

            {/* Bendri laukai */}
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

            {/* Gamintojas: visos kategorijos, išskyrus Furniture */}
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

            {/* Drabužių dydis */}
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

            {/* Avalynės dydis */}
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

            {/* Spalva */}
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

            {/* Elektronikos ir transporto modelis */}
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

            {/* Baldų medžiaga */}
            {category === "Furniture" && (
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
            )}

            {/* Baldų matmenys */}
            {category === "Furniture" && (
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
            )}

            {/* Transporto ir instrumento pagaminimo metai */}
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

            {/* Muzikos instrumento tipas */}
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