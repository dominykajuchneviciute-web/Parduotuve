import { useEffect, useState } from "react";
import { Link, useParams } from "react-router-dom";
import type { ChatMessage } from "../types";
import * as signalR from "@microsoft/signalr";
import { useAuth } from "../AuthContext";

const API = "http://localhost:5145/api/Chat";

export default function ChatDetails() {
    const { conversationId } = useParams<{ conversationId: string }>();
    const { user } = useAuth();
    const currentUserId = user?.id;
    const [messages, setMessages] = useState<ChatMessage[]>([]);
    const [text, setText] = useState("");
    const [loading, setLoading] = useState(true);
    const [sending, setSending] = useState(false);
    const [error, setError] = useState<string | null>(null);
    useEffect(() => {
        if (!conversationId) return;

        const connection = new signalR.HubConnectionBuilder()
            .withUrl("http://localhost:5145/hubs/chat", {
                withCredentials: true,
            })
            .withAutomaticReconnect()
            .build();

        const handleReceiveMessage = (message: ChatMessage) => {
            setMessages((currentMessages) => {
               
                if (currentMessages.some(m => m.id === message.id)) {
                    return currentMessages;
                }

                return [...currentMessages, message];
            });
        };

        connection.on("ReceiveMessage", handleReceiveMessage);

        async function startConnection() {
            try {
                await connection.start();

                await connection.invoke(
                    "JoinConversation",
                    Number(conversationId)
                );
            } catch (error) {
                console.error("SignalR prisijungimo klaida:", error);
            }
        }

        connection.onreconnected(async () => {
            try {
                await connection.invoke(
                    "JoinConversation",
                    Number(conversationId)
                );
            } catch (error) {
                console.error("Nepavyko vėl prisijungti prie pokalbio:", error);
            }
        });

        startConnection();

        return () => {
            connection.off("ReceiveMessage", handleReceiveMessage);
            connection.stop();
        };
    }, [conversationId]);

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

                const data: ChatMessage[] = await res.json();

                setMessages(currentMessages => {
                    const messagesMap = new Map(
                        currentMessages.map(message => [message.id, message])
                    );

                    for (const message of data) {
                        messagesMap.set(message.id, message);
                    }

                    return Array.from(messagesMap.values()).sort(
                        (a, b) =>
                            new Date(a.sentAt).getTime() -
                            new Date(b.sentAt).getTime()
                    );
                });
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
                messages.map((message) => {
                    const isMine = message.senderId === currentUserId;

                    return (
                        <div
                    key= { message.id }
                    className = {`d-flex mb-3 ${isMine
                            ? "justify-content-end"
                            : "justify-content-start"
                        }`
                }
                >
                    <div
                        className={`p-3 rounded ${isMine
                        ? "bg-primary text-white"
                        : "bg-light"
                    }`}
                    style = {{ maxWidth: "70%" }}
                    >
                    <div>{ message.text } </div>

                    < small
                            className = {
                        isMine
                        ? "text-white-50"
                            : "text-muted"
                    }
                    >
                    { new Date(message.sentAt).toLocaleString() }
                    </small>
                    </div>
                    </div>
                );
        })
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
onChange = {(event) => setText(event.target.value)}
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