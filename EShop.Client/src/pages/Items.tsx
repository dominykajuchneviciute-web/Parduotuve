import { useEffect, useState } from "react";
import { Link } from "react-router-dom";
import type { ItemDto } from "../types";

export default function Items() {
  const [allItems, setAllItems] = useState<ItemDto[]>([]);
  const [isLoading, setIsLoading] = useState(true);
  const [errorMessage, setErrorMessage] = useState("");
  const [searchQuery, setSearchQuery] = useState("");
  const [selectedCategory, setSelectedCategory] = useState("");
  const [selectedExchangeType, setSelectedExchangeType] = useState("");

  useEffect(() => {
    fetch("http://localhost:5000/api/items")
      .then((res) => {
        if (!res.ok) throw new Error("Klaida gaunant duomenis");
        return res.json();
      })
      .then((data) => {
        setAllItems(data);
        setIsLoading(false);
      })
      .catch((err) => {
        setErrorMessage("Nepavyko pasiekti API serverio: " + err.message);
        setIsLoading(false);
      });
  }, []);

  const filteredItems = allItems.filter((i) => {
    const matchesSearch = !searchQuery || i.title.toLowerCase().includes(searchQuery.toLowerCase());
    const matchesCategory = !selectedCategory || i.category === selectedCategory;
    const matchesExchange = !selectedExchangeType || i.exchangeType.toLowerCase().includes(selectedExchangeType.toLowerCase());
    return matchesSearch && matchesCategory && matchesExchange;
  });

  const resetFilters = () => {
    setSearchQuery("");
    setSelectedCategory("");
    setSelectedExchangeType("");
  };

  return (
    <div className="container mt-4">
      <div className="row">
        {/* Filtrai */}
        <div className="col-md-3">
          <div className="card p-3 shadow-sm mb-4">
            <h5>Filtrai</h5>
            <hr />
            <div className="mb-3">
              <label className="form-label">Paieška:</label>
              <input
                type="text"
                className="form-control"
                placeholder="Ieškoti pagal pavadinimą..."
                value={searchQuery}
                onChange={(e) => setSearchQuery(e.target.value)}
              />
            </div>
            <div className="mb-3">
              <label className="form-label">Kategorija:</label>
              <select className="form-select" value={selectedCategory} onChange={(e) => setSelectedCategory(e.target.value)}>
                <option value="">Visos kategorijos</option>
                <option value="Transportas">Transportas</option>
                <option value="Elektronika">Elektronika</option>
                <option value="Knygos">Knygos</option>
                <option value="Buitis">Buitis</option>
              </select>
            </div>
            <div className="mb-3">
              <label className="form-label">Mainų būdas:</label>
              <select className="form-select" value={selectedExchangeType} onChange={(e) => setSelectedExchangeType(e.target.value)}>
                <option value="">Visi būdai</option>
                <option value="Gyvai">Gyvai</option>
                <option value="Siuntimu">Siuntimu</option>
              </select>
            </div>
            <button className="btn btn-outline-secondary w-100" onClick={resetFilters}>Išvalyti filtrus</button>
          </div>
        </div>

        {/* Sąrašas */}
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
                    {item.category}
                  </div>
                  <div className="card-body d-flex flex-column">
                    <h5 className="card-title">{item.title}</h5>
                    <div className="mb-2">
                      <span className="badge bg-secondary me-1">{item.category}</span>
                      <span className="badge bg-info text-dark">{item.exchangeType}</span>
                    </div>
                    <p className="card-text text-muted small flex-grow-1">{item.description}</p>
                    <div className="bg-light p-2 rounded mb-3 small">
                      <strong>Keičia į:</strong> {item.lookingFor}
                    </div>
                    <Link to={`/item/${item.id}`} className="btn btn-outline-primary btn-sm w-100">
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