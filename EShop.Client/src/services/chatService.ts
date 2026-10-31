
const API = "http://localhost:5145/api/Chat";


export interface Conversation {
    id: number;
    user1Id: string;
    user2Id: string;
    itemId: number;
}


export interface Message {
    id: number;
    conversationId: number;
    senderId: string;
    text: string;
    sentAt: string;
}
async function handleResponse<T>(response: Response): Promise<T> {
    if (!response.ok) {
        const error = await response.text();
        throw new Error(error || `HTTP klaida: ${response.status}`);
    }

    return response.json() as Promise<T>;
}


export async function createConversation(
    itemId: number
): Promise<{ id: number }> {
    const response = await fetch(
        `${API}/conversations?itemId=${itemId}`,
        {
            method: "POST",
            credentials: "include",
        }
    );

    return handleResponse<{ id: number }>(response);
}


export async function getConversations(): Promise<Conversation[]> {
    const response = await fetch(`${API}/conversations`, {
        credentials: "include",
    });

    return handleResponse<Conversation[]>(response);
}


export async function getMessages(
    conversationId: number
): Promise<Message[]> {
    const response = await fetch(
        `${API}/conversations/${conversationId}/messages`,
        {
            credentials: "include",
        }
    );

    return handleResponse<Message[]>(response);
}


export async function sendMessage(
    conversationId: number,
    text: string
): Promise<Message> {
    const response = await fetch(
        `${API}/conversations/${conversationId}/messages`,
        {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
            },
            credentials: "include",
            body: JSON.stringify({ text }),
        }
    );

    return handleResponse<Message>(response);
}
