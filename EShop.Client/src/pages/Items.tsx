import { useEffect, useState } from "react";
import { Link } from "react-router-dom";
import type { ItemDto } from "../types";

export default function Items() {
  const [allItems, setAllItems] = useState<ItemDto[]>([]);
  const [isLoading, setIsLoading] = useState(true);
  const [errorMessage, setErrorMessage] = useState("");
  
  const [searchQuery, setSearchQuery] = useState("");
  const [selectedCondition, setSelectedCondition] = useState("");

  useEffect(() => {
     fetch("http://localhost:5145/api/items")
      .then((res) => {
        if (!res.ok) throw new Error("Klaida gaunant duomenis");
        return res.json();
      })
      .then((data) => {
        setAllItems(data);
        setIsLoading(false);
      })
      .catch((err) => {
        setErrorMessage("Nepavyko pasiekti serverio: " + err.message);
        setIsLoading(false);
      });
  }, []);

    const conditionLabels: Record<string, string> = {
        "0": "New",
        "1": "VeryGood",
        "2": "Good",
        "3": "Decent",
        "4": "Used",
        "5": "VeryUsed",
    };

    const filteredItems = allItems.filter((i) => {
        const matchesSearch =
            !searchQuery ||
            i.name.toLowerCase().includes(searchQuery.toLowerCase());

        const expectedCondition = conditionLabels[selectedCondition];

        const matchesCondition =
            !selectedCondition ||
            i.condition?.toString() === selectedCondition ||
            i.condition?.toString() === expectedCondition;

        return matchesSearch && matchesCondition;
    });

  const resetFilters = () => {
    setSearchQuery("");
    setSelectedCondition("");
  };
    const getCategoryLabel = (item: ItemDto): string => {
        switch (item.itemType) {
            case "Clothing":
                return "Drabužiai";
            case "Electronics":
                return "Elektronika";
            case "Furniture":
                return "Baldai";
            case "Transport":
                return "Transportas";
            case "Footwear":
                return "Avalynė";
            case "Instrument":
                return "Muzikos instrumentai";
            default:
                return "Daiktas";
        }
    };
  return (
    <div className="container mt-4">
      <div className="row">
        <div className="col-md-3">
          <div className="card p-3 shadow-sm mb-4">
            <h5>Filtrai</h5>
            <hr />
            <div className="mb-3">
              <label className="form-label">Paieška pagal pavadinimą:</label>
              <input
                type="text"
                className="form-control"
                placeholder="Ieškoti..."
                value={searchQuery}
                onChange={(e) => setSearchQuery(e.target.value)}
              />
            </div>

            <div className="mb-3">
              <label className="form-label">Būklė (Condition):</label>
              <select 
                className="form-select" 
                value={selectedCondition} 
                onChange={(e) => setSelectedCondition(e.target.value)}
              >
                <option value="">Visos būklės</option>
                <option value="0">Naujas</option>
                <option value="1">Labai geras</option>
                <option value="2">Geras</option>
                <option value="3">Patenkinamas</option>
                <option value="4">Naudotas</option>
                <option value="5">Labai naudotas</option>
              </select>
            </div>

            {/* Uzkomentavau filtrus kol nera backende
            <div className="mb-3">
              <label className="form-label">Kategorija:</label>
              <select className="form-select" ...>...</select>
            </div>
            <div className="mb-3">
              <label className="form-label">Mainų būdas:</label>
              <select className="form-select" ...>...</select>
            </div>
            */}

            <button className="btn btn-outline-secondary w-100" onClick={resetFilters}>Išvalyti filtrus</button>
          </div>
        </div>

        <div className="col-md-9">
          <div className="d-flex justify-content-between align-items-center mb-3">
            <h3>Visi siūlomi daiktai</h3>
            <Link to="/my-items" className="btn btn-success">+ Siūlyti daiktą</Link>
          </div>

          {isLoading && <div className="alert alert-info">Kraunami daiktai iš serverio...</div>}
          {errorMessage && <div className="alert alert-danger">{errorMessage}</div>}
          {!isLoading && !errorMessage && filteredItems.length === 0 && (
            <div className="alert alert-warning">Pagal nurodytus filtrus daiktų nerasta.</div>
          )}

          <div className="row">
            {filteredItems.map((item) => (
              <div key={item.id} className="col-md-4 mb-4">
                <div className="card h-100 shadow-sm">
                  <div style={{ height: "120px", backgroundColor: "#f8f9fa" }} className="d-flex align-items-center justify-content-center text-muted border-bottom">
                    {item.manufacturer || "Prekė"}
                  </div>
                  <div className="card-body d-flex flex-column">
                    <h5 className="card-title" > { item.name } </h5>

                        < div className = "mb-2" >
                            <span className="badge bg-primary me-1" >
                            { getCategoryLabel(item) }
                                </span>

                    {
                        item.condition != null && (
                            <span className="badge bg-secondary me-1" >
                            { item.condition }
                                </span>
                      )
                    }

                    {
                        item.size != null && (
                            <span className="badge bg-info text-dark me-1" >
                                Dydis: { item.size }
                        </span>
                      )
                    }

                    {
                        item.shoeSize != null && (
                            <span className="badge bg-info text-dark me-1" >
                                Avalynės dydis: { item.shoeSize }
                        </span>
                      )
                    }
                    </div>

                    {
                        item.manufacturer && (
                            <p className="small mb-1" >
                                Gamintojas: { item.manufacturer }
                        </p>
                    )
                    }

                    {
                        item.color && (
                            <p className="small mb-1" >
                                Spalva: { item.color }
                        </p>
                    )
                    }

                    {
                        item.model && (
                            <p className="small mb-1" >
                                Modelis: { item.model }
                        </p>
                    )
                    }

                    {
                        item.material && (
                            <p className="small mb-1" >
                                Medžiaga: { item.material }
                        </p>
                    )
                    }

                    {
                        item.dimensions && (
                            <p className="small mb-1" >
                                Matmenys: { item.dimensions }
                        </p>
                    )
                    }

                    {
                        item.year != null && (
                            <p className="small mb-1" >
                                Metai: { item.year }
                        </p>
                    )
                    }

                    {
                        item.instrumentType && (
                            <p className="small mb-1" >
                                Instrumento tipas: { item.instrumentType }
                        </p>
                    )
                    }

                    <p className="card-text text-muted small flex-grow-1" >
                    { item.description }
                        </p>
                    <Link to={`/item/${item.id}`} className="btn btn-outline-primary btn-sm w-100 mt-2">
                      Peržiūrėti pasiūlymą
                    </Link>
                  </div>
                </div>
              </div>
            ))}
          </div>
        </div>
      </div>
    </div>
  );
}