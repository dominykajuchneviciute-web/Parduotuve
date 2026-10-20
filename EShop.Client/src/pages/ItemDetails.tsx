import { useEffect, useState } from "react";
import { useParams, Link } from "react-router-dom";
import type { ItemDto, ItemCategory } from "../types";
import { ItemInteractions } from "../components/ItemInteractions";

const categoryNames: Record<ItemCategory, string> = {
    Clothing: "Drabužiai",
    Electronics: "Elektronika",
    Furniture: "Baldai",
    Transport: "Transportas",
    Footwear: "Avalynė",
    Instrument: "Muzikos instrumentai",
};

const conditionLabels: Record<string, string> = {
    "0": "Naujas",
    "1": "Labai geras",
    "2": "Geras",
    "3": "Patenkinamas",
    "4": "Naudotas",
    "5": "Labai naudotas",
    New: "Naujas",
    VeryGood: "Labai geras",
    Good: "Geras",
    Decent: "Patenkinamas",
    Used: "Naudotas",
    VeryUsed: "Labai naudotas",
};

function getConditionLabel(condition: string | number): string {
    return conditionLabels[String(condition)] ?? String(condition);
}

export default function ItemDetails() {
    const { id } = useParams<{ id: string }>();

    const [item, setItem] = useState<ItemDto | null>(null);
    const [isLoading, setIsLoading] = useState(true);
    const [showProposalAlert, setShowProposalAlert] = useState(false);
    const [isSaved, setIsSaved] = useState(false);
    const [isSavingLoading, setIsSavingLoading] = useState(false);

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

fetch(`http://localhost:5145/api/items/${id}/saved`, {
    credentials: "include",
})
    .then((res) => (res.ok ? res.json() : null))
    .then((data) => {
        if (data && typeof data.isSaved === "boolean") {
            setIsSaved(data.isSaved);
        }
    })
    .catch((err) =>
        console.error("Klaida tikrinant įsiminimo būseną:", err)
    );
    }, [id]);

const handleToggleSave = async () => {
    setIsSavingLoading(true);

    try {
        const res = await fetch(
            `http://localhost:5145/api/items/${id}/saved/toggle`,
            {
                method: "POST",
                credentials: "include",
            }
        );

        if (res.status === 401) {
            alert("Norėdami įsiminti daiktą, pirmiausia prisijunkite!");
            return;
        }

        if (res.ok) {
            const data = await res.json();
            setIsSaved(data.isSaved);
        }
    } catch (error) {
        console.error("Klaida keičiant įsiminimo būseną:", error);
    } finally {
        setIsSavingLoading(false);
    }
};

const category = item?.itemType;

function renderDetail(
    label: string,
    value: string | number | null | undefined
) {
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
            Daiktas su ID: { id } nerastas arba nepavyko gauti jo
    informacijos.
                </div>
            )
}

{
    item && (
        <div className="card p-4 shadow-sm" >
            <div className="row" >
                <div
                            className="col-md-6 d-flex align-items-center justify-content-center bg-light mb-3 mb-md-0"
    style = {{
        minHeight: "280px",
            borderRadius: "8px",
                            }
}
                        >
    <span className="fs-4 text-muted" >
        Nuotraukos vieta
            </span>
            </div>

            < div className = "col-md-6" >
                <h2>{ item.name } </h2>

{
    category && (
        <p className="text-muted" >
            <strong>Kategorija: </strong>{" "}
    {
        categoryNames[category as ItemCategory] ??
        category
    }
    </p>
                            )
}

<hr />

    < h5 className = "mb-3" > Pagrindinė informacija </h5>

{ renderDetail("Daikto ID", item.id) }
{
    renderDetail(
        "Būklė",
        getConditionLabel(item.condition)
    )
}
{ renderDetail("Aprašymas", item.description) }

<hr />

    < h5 className = "mb-3" > Daikto savybės </h5>

{
    category === "Clothing" && (
        <>
        { renderDetail("Dydis", item.size) }
                                    { renderDetail("Spalva", item.color) }
    {
        renderDetail(
            "Gamintojas",
            item.manufacturer
        )
    }
    </>
                            )
}

{
    category === "Electronics" && (
        <>
        {
            renderDetail(
                                        "Gamintojas",
                item.manufacturer
                                    )
        }
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
        {
            renderDetail(
                                        "Gamintojas",
                item.manufacturer
                                    )
        }
                                    { renderDetail("Modelis", item.model) }
    {
        renderDetail(
            "Pagaminimo metai",
            item.year
        )
    }
    </>
                            )
}

{
    category === "Footwear" && (
        <>
        { renderDetail("Batų dydis", item.shoeSize) }
                                    { renderDetail("Spalva", item.color) }
    {
        renderDetail(
            "Gamintojas",
            item.manufacturer
        )
    }
    </>
                            )
}

{
    category === "Instrument" && (
        <>
        {
            renderDetail(
                                        "Instrumento tipas",
                item.instrumentType
                                    )
        }
                                    {
        renderDetail(
            "Gamintojas",
            item.manufacturer
        )
    }
    {
        renderDetail(
            "Pagaminimo metai",
            item.year
        )
    }
    </>
                            )
}

{
    !category && (
        <>
        { renderDetail("Dydis", item.size) }
                                    { renderDetail("Batų dydis", item.shoeSize) }
    { renderDetail("Spalva", item.color) }
    {
        renderDetail(
            "Gamintojas",
            item.manufacturer
        )
    }
    { renderDetail("Modelis", item.model) }
    { renderDetail("Medžiaga", item.material) }
    { renderDetail("Matmenys", item.dimensions) }
    {
        renderDetail(
            "Pagaminimo metai",
            item.year
        )
    }
    {
        renderDetail(
            "Instrumento tipas",
            item.instrumentType
        )
    }
    </>
                            )
}

<hr />

    < div className = "d-flex align-items-center" >
        <button
                                    className="btn btn-success me-2"
onClick = {() =>
setShowProposalAlert(true)
                                    }
                                >
                                    🤝 Siūlyti mainus
    </button>

    < button
className = {`btn ${isSaved
        ? "btn-danger"
        : "btn-outline-danger"
    }`}
onClick = { handleToggleSave }
disabled = { isSavingLoading }
    >
{ isSaved? "❤️ Įsiminta": "Įsiminti" }
    </button>
    </div>

{
    showProposalAlert && (
        <div className="alert alert-success mt-3 py-2" >
            Pasiūlymas išsiųstas! Savininkas su jumis
    susisieks.
                                </div>
                            )
}

<ItemInteractions itemId={ item.id } />
    </div>
    </div>
    </div>
            )}
</div>
    );
}