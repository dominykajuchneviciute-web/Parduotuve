import { useEffect, useState } from "react";
import { Link, useParams } from "react-router-dom";
import type { ChatMessage } from "../types";

const API = "http://localhost:5145/api/Chat";

export default function ChatDetails() {
    const { conversationId } = useParams<{ conversationId: string }>();

    const [messages, setMessages] = useState<ChatMessage[]>([]);
    const [text, setText] = useState("");
    const [loading, setLoading] = useState(true);
    const [sending, setSending] = useState(false);
    const [error, setError] = useState<string | null>(null);

    useEffect(() => {
        async function loadMessages() {
            try {
                const res = await fetch(
                    `${API}/conversations/${conversationId}/messages`,
                    {
                        credentials: "include",
                    }
                );

                if (!res.ok) {
                    throw new Error("Nepavyko gauti žinučių.");
                }

                const data = await res.json();
                setMessages(data);
            } catch (error) {
                console.error("Klaida gaunant žinutes:", error);
                setError("Nepavyko gauti žinučių.");
            } finally {
                setLoading(false);
            }
        }

        loadMessages();
    }, [conversationId]);

    async function handleSendMessage(
        event: React.FormEvent
    ) {
        event.preventDefault();

        if (!text.trim()) {
            return;
        }

        setSending(true);

        try {
            const res = await fetch(
                `${API}/conversations/${conversationId}/messages`,
                {
                    method: "POST",
                    headers: {
                        "Content-Type": "application/json",
                    },
                    credentials: "include",
                    body: JSON.stringify({
                        text: text.trim(),
                    }),
                }
            );

            if (!res.ok) {
                throw new Error("Nepavyko išsiųsti žinutės.");
            }

            const newMessage: ChatMessage = await res.json();

            setMessages((currentMessages) => [
                ...currentMessages,
                newMessage,
            ]);

            setText("");
        } catch (error) {
            console.error("Klaida siunčiant žinutę:", error);
            setError("Nepavyko išsiųsti žinutės.");
        } finally {
            setSending(false);
        }
    }

    if (loading) {
        return (
            <div className= "container mt-4" >
            <div className="alert alert-info" >
                Kraunamos žinutės...
        </div>
            </div>
        );
    }

    return (
        <div className= "container mt-4" >
        <Link
                to="/chats"
    className = "btn btn-secondary btn-sm mb-3"
        >
                ← Atgal į pokalbius
        </Link>

        < h2 className = "mb-4" >
                💬 Pokalbis
        </h2>

    {
        error && (
            <div className="alert alert-danger" >
            { error }
                </div>
            )
    }

    <div
                className="card p-3 mb-3"
    style = {{
        minHeight: "350px",
            maxHeight: "500px",
                overflowY: "auto",
                }
}
            >
    {
        messages.length === 0 ? (
            <p className= "text-muted text-center" >
            Pokalbyje dar nėra žinučių.
                    </p>
                ) : (
                messages.map((message) => (
                    <div
                            key= { message.id }
                            className = {`d-flex mb-3 ${message.isMine
                            ? "justify-content-end"
                            : "justify-content-start"
                        }`}
                        >
                    <div
                                className={`p-3 rounded ${message.isMine
                        ? "bg-primary text-white"
                        : "bg-light"
                    }`}
                    style = {{
                    maxWidth: "70%",
                }}
                            >
                    <div>
                    { message.text }
                    </div>

                    < small
                                    className = {
                        message.isMine
                            ? "text-white-50"
                            : "text-muted"
                    }
                    >
                    {
                        new Date(
                            message.sentAt
                        ).toLocaleString()
                    }
                    </small>
                    </div>
                    </div>
                ))
                )}
</div>

    < form
onSubmit = { handleSendMessage }
className = "d-flex gap-2"
    >
    <input
                    type="text"
className = "form-control"
placeholder = "Rašykite žinutę..."
value = { text }
onChange = {(event) =>
setText(event.target.value)
                    }
disabled = { sending }
    />

    <button
                    type="submit"
className = "btn btn-primary"
disabled = { sending || !text.trim()}
                >
{ sending? "Siunčiama...": "Siųsti" }
    </button>
    </form>
    </div>
    );
}