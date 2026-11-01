
export type ItemCategory =
    | "Clothing"
    | "Electronics"
    | "Furniture"
    | "Transport"
    | "Footwear"
    | "Instrument";

export interface ItemFilterState {
    searchQuery: string;
    selectedCondition: string;
    selectedCategory: ItemCategory | "";
    manufacturer: string;
    model: string;
    color: string;
    material: string;
    dimensions: string;
    instrumentType: string;
    size: string;
    shoeSize: string;
    year: string;
}

export interface ItemDto {
    id: number;
    name: string;
    condition: number | string;
    description?: string | null;
    ownerId?: string;

    itemType?: ItemCategory;

    size?: number | null;
    color?: string | null;
    manufacturer?: string | null;

    model?: string | null;

    material?: string | null;
    dimensions?: string | null;

    year?: number | null;

    shoeSize?: number | null;

    instrumentType?: string | null;
}
export interface Conversation {
    id: number;
    user1Id: string;
    user2Id: string;
    itemId: number;
}

export interface ChatMessage {
    id: number;
    conversationId: number;
    senderId: string;
    text: string;
    sentAt: string;
    isMine?: boolean;
}

export interface Conversation {
    id: number;
    user1Id: string;
    user2Id: string;
    itemId: number;
}

export interface ChatMessage {
    id: number;
    conversationId: number;
    senderId: string;
    text: string;
    sentAt: string;
}