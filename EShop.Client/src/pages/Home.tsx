
import { useEffect, useState } from "react";
import { Link } from "react-router-dom";

interface Item {
    id: number | string;
    name: string;
    description?: string;
    condition?: number | string;
    itemType?: string;
    category?: string;
}

export default function Home() {
    const [items, setItems] = useState<Item[]>([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState("");

    useEffect(() => {
        async function fetchItems() {
            try {
                const response = await fetch(
                    "http://localhost:5145/api/items"
                );

                if (!response.ok) {
                    throw new Error("Nepavyko gauti daiktų sąrašo.");
                }

                const data: Item[] = await response.json();
                setItems(data.slice(0, 4));
            } catch (err) {
                setError(
                    err instanceof Error
                        ? err.message
                        : "Įvyko nežinoma klaida."
                );
            } finally {
                setLoading(false);
            }
        }

        fetchItems();
    }, []);

    return (
        <div className= "container mt-4" >
        <div className="p-5 mb-4 bg-light rounded-3 text-center border shadow-sm" >
            <h1 className="display-5 fw-bold" >
                Daiktų mainų platforma
                    </h1>

                    < p className = "fs-5 text-muted" >
                        Apsikeiskite nereikalingais daiktais be jokių
    pinigų – gyvai arba siuntimu!
        </p>

        < div className = "d-flex justify-content-center gap-3 mt-3" >
            <Link to="/items" className = "btn btn-primary btn-lg" >
                Naršyti daiktus
                    </Link>

                    < Link
    to = "/my-items"
    className = "btn btn-outline-secondary btn-lg"
        >
        Pasiūlyti mainams
            </Link>
            </div>
            </div>

            < h3 className = "mt-5 mb-3" >
                Naujausi mainų pasiūlymai
                    </h3>

    {
        loading && (
            <p>Kraunami daiktų pasiūlymai...</p>
      )
    }

    {
        error && (
            <div className="alert alert-danger" >
            { error }
                </div>
      )
    }

    {
        !loading && !error && items.length === 0 && (
            <div className="alert alert-info" >
                Kol kas pasiūlymų nėra.
        </div>
      )
    }

    {
        !loading && !error && items.length > 0 && (
            <div className="row" >
            {
                items.map((item) => (
                    <div
              className= "col-md-6 mb-3"
              key = { item.id }
                    >
                    <div className="card h-100 shadow-sm border-start border-4" >
                <div className="card-body" >
                <h5 className="card-title" >
                { item.name }
                </h5>

                  {
                        item.itemType && (
                            <p className="text-muted small mb-2">
                                Kategorija: { item.itemType }
                </p>
                )
            }

        {
            item.description && (
                <p className="mb-3" >
                { item.description }
                    </p>
                  )
        }

        <Link
                    to={ `/item/${item.id}` }
        className = "btn btn-sm btn-outline-primary"
            >
            Peržiūrėti daiktą
                </Link>
                </div>
                </div>
                </div>
          ))
    }
    </div>
      )
}
</div>
  );
}
