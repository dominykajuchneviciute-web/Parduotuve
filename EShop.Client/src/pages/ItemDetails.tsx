import { useEffect, useState } from "react";
import { useParams, Link } from "react-router-dom";
import type { ItemDto, ItemCategory } from "../types";

const categoryNames: Record<ItemCategory, string> = {
  Clothing: "Drabužiai",
  Electronics: "Elektronika",
  Furniture: "Baldai",
  Transport: "Transportas",
  Footwear: "Avalynė",
  Instrument: "Muzikos instrumentai",
};

export default function ItemDetails() {
  const { id } = useParams<{ id: string }>();

  const [item, setItem] = useState<ItemDto | null>(null);
  const [isLoading, setIsLoading] = useState(true);
  const [showProposalAlert, setShowProposalAlert] = useState(false);

  useEffect(() => {
    setIsLoading(true);

    fetch(`http://localhost:5145/api/items/${id}`)
      .then((res) => {
    if (!res.ok) {
        throw new Error("Nepavyko gauti daikto informacijos");
    }

    return res.json();
})
    .then((data: ItemDto) => {
        setItem(data);
    })
    .catch((error) => {
        console.error("Klaida gaunant daiktą:", error);
        setItem(null);
    })
    .finally(() => {
        setIsLoading(false);
    });
  }, [id]);

// API atsakyme kategorija gali būti pateikta kaip itemType
// arba category, priklausomai nuo backend realizacijos.
const category = item?.itemType ?? item?.category;

function renderDetail(label: string, value: string | number | null | undefined) {
    if (value === null || value === undefined || value === "") {
        return null;
    }

    return (
        <p key= { label } className = "mb-2" >
            <strong>{ label }: </strong> {value}
                </p>
    );
}

return (
    <div className= "container mt-4" >
    <Link to="/items" className = "btn btn-secondary btn-sm mb-3" >
        ← Atgal į daiktų sąrašą
    </Link>

{
    isLoading && (
        <div className="alert alert-info" >
            Kraunama informacija apie daiktą...
    </div>
      )
}

{
    !isLoading && !item && (
        <div className="alert alert-warning" >
            Daiktas su ID: { id } nerastas arba nepavyko gauti jo informacijos.
        </div>
      )
}

{
    item && (
        <div className="card p-4 shadow-sm" >
            <div className="row" >
                <div
              className="col-md-6 d-flex align-items-center justify-content-center bg-light mb-3 mb-md-0"
    style = {{ minHeight: "280px", borderRadius: "8px" }
}
            >
    <span className="fs-4 text-muted" > Nuotraukos vieta </span>
        </div>

        < div className = "col-md-6" >
            <h2>{ item.name } </h2>

{
    category && (
        <p className="text-muted" >
            <strong>Kategorija: </strong>{" "}
    { categoryNames[category as ItemCategory] ?? category }
    </p>
              )
}

<hr />

    < h5 className = "mb-3" > Pagrindinė informacija </h5>

{ renderDetail("Daikto ID", item.id) }
{ renderDetail("Būklė", item.condition) }
{ renderDetail("Aprašymas", item.description) }

<hr />

    < h5 className = "mb-3" > Daikto savybės </h5>

{
    category === "Clothing" && (
        <>
        { renderDetail("Dydis", item.size) }
                  { renderDetail("Spalva", item.color) }
    { renderDetail("Gamintojas", item.manufacturer) }
    </>
              )
}

{
    category === "Electronics" && (
        <>
        { renderDetail("Gamintojas", item.manufacturer) }
                  { renderDetail("Modelis", item.model) }
    { renderDetail("Spalva", item.color) }
    </>
              )
}

{
    category === "Furniture" && (
        <>
        { renderDetail("Medžiaga", item.material) }
                  { renderDetail("Matmenys", item.dimensions) }
    { renderDetail("Spalva", item.color) }
    </>
              )
}

{
    category === "Transport" && (
        <>
        { renderDetail("Gamintojas", item.manufacturer) }
                  { renderDetail("Modelis", item.model) }
    { renderDetail("Pagaminimo metai", item.year) }
    </>
              )
}

{
    category === "Footwear" && (
        <>
        { renderDetail("Batų dydis", item.shoeSize) }
                  { renderDetail("Spalva", item.color) }
    { renderDetail("Gamintojas", item.manufacturer) }
    </>
              )
}

{
    category === "Instrument" && (
        <>
        { renderDetail("Instrumento tipas", item.instrumentType) }
                  { renderDetail("Gamintojas", item.manufacturer) }
    { renderDetail("Pagaminimo metai", item.year) }
    </>
              )
}

{
    !category && (
        <>
        { renderDetail("Dydis", item.size) }
                  { renderDetail("Batų dydis", item.shoeSize) }
    { renderDetail("Spalva", item.color) }
    { renderDetail("Gamintojas", item.manufacturer) }
    { renderDetail("Modelis", item.model) }
    { renderDetail("Medžiaga", item.material) }
    { renderDetail("Matmenys", item.dimensions) }
    { renderDetail("Pagaminimo metai", item.year) }
    { renderDetail("Instrumento tipas", item.instrumentType) }
    </>
              )
}

<hr />

    < button
className = "btn btn-success me-2"
onClick = {() => setShowProposalAlert(true)}
              >
                🤝 Siūlyti mainus
    </button>

    < button className = "btn btn-outline-danger" >
        Įsiminti
        </button>

{
    showProposalAlert && (
        <div className="alert alert-success mt-3 py-2" >
            Pasiūlymas išsiųstas! Savininkas su jumis susisieks.
                </div>
              )
}
</div>
    </div>
    </div>
      )}
</div>
  );
}
