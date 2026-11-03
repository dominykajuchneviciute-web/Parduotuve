
import { useEffect, useState } from "react";
import { Link } from "react-router-dom";
import type { Conversation } from "../types";

const API = "http://localhost:5145/api/Chat";

export default function Chats() {
    const [conversations, setConversations] = useState<Conversation[]>([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState<string | null>(null);
    const [deletingId, setDeletingId] = useState<number | null>(null);

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

    async function handleDeleteConversation(conversationId: number) {
        const confirmed = window.confirm(
            "Ar tikrai norite ištrinti šį pokalbį? Jis bus pašalintas tik iš jūsų pokalbių sąrašo."
        );

        if (!confirmed) return;

        setDeletingId(conversationId);
        setError(null);

        try {
            const res = await fetch(
                `${API}/conversations/${conversationId}`,
                {
                    method: "DELETE",
                    credentials: "include",
                }
            );

            if (!res.ok) {
                const message = await res.text();
                throw new Error(message || "Nepavyko ištrinti pokalbio.");
            }

            setConversations((current) =>
                current.filter(
                    (conversation) => conversation.id !== conversationId
                )
            );
        } catch (error) {
            console.error("Klaida trinant pokalbį:", error);
            setError(
                error instanceof Error
                    ? error.message
                    : "Nepavyko ištrinti pokalbio."
            );
        } finally {
            setDeletingId(null);
        }
    }

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
            <div
                        key= { conversation.id }
                        className = "list-group-item d-flex justify-content-between align-items-center gap-3"
            >
            <Link
                            to={`/chats/${conversation.id}`}
    className = "text-decoration-none text-dark flex-grow-1"
        >
        <div className="d-flex justify-content-between" >
            <strong>
            Pokalbis #{ conversation.id }
    </strong>

        < span className = "text-muted" >
        { conversation.itemName ?? `Prekė #${conversation.itemId}` }
            </span>
        </div>
        </Link>

        < button
    type = "button"
    className = "btn btn-outline-danger btn-sm"
    onClick = {() =>
    handleDeleteConversation(conversation.id)
}
disabled = { deletingId !== null}
                        >
{ deletingId === conversation.id
    ? "Trinama..."
    : "🗑️ Ištrinti"}
</button>
    </div>
                ))}
</div>
    </div>
    );
}
