export interface BookingDto {
    id?: number;
    roomId: number;
    guestId: number;
    money: number;
    isPaid: boolean;
    numberPeople: number;
    startDate: Date;
    endDate: Date;
}