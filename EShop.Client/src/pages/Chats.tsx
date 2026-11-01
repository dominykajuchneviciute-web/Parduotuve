import { useEffect, useState } from "react";
import { Link } from "react-router-dom";
import type { Conversation } from "../types";

const API = "http://localhost:5145/api/Chat";

export default function Chats() {
    const [conversations, setConversations] = useState<Conversation[]>([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState<string | null>(null);

    useEffect(() => {
        async function loadConversations() {
            try {
                const res = await fetch(`${API}/conversations`, {
                    credentials: "include",
                });

                if (res.status === 401) {
                    setError("Norėdami peržiūrėti pokalbius, prisijunkite.");
                    return;
                }

                if (!res.ok) {
                    throw new Error("Nepavyko gauti pokalbių.");
                }

                const data = await res.json();
                setConversations(data);
            } catch (error) {
                console.error("Klaida gaunant pokalbius:", error);
                setError("Nepavyko gauti pokalbių.");
            } finally {
                setLoading(false);
            }
        }

        loadConversations();
    }, []);

    if (loading) {
        return (
            <div className= "container mt-4" >
            <div className="alert alert-info" >
                Kraunami pokalbiai...
        </div>
            </div>
        );
    }

    return (
        <div className= "container mt-4" >
        <h2 className="mb-4" >💬 Pokalbiai </h2>

    {
        error && (
            <div className="alert alert-warning" >
            { error }
                </div>
            )
    }

    {
        !error && conversations.length === 0 && (
            <div className="alert alert-info" >
                Dar neturite pokalbių.
                </div>
            )
    }

    <div className="list-group" >
    {
        conversations.map((conversation) => (
            <Link
                        key= { conversation.id }
                        to = {`/chats/${conversation.id}`}
    className = "list-group-item list-group-item-action"
        >
        <div className="d-flex justify-content-between" >
            <strong>
            Pokalbis #{ conversation.id }
    </strong>

        < span className = "text-muted" >
            Prekė #{ conversation.itemId }
    </span>
        </div>
        </Link>
                ))
}
</div>
    </div>
    );
}