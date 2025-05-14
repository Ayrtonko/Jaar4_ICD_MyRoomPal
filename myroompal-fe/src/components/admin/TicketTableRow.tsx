import { TicketData } from "../../models/AdminPanel/TicketData";

interface TicketTableRowProps {
    ticketinfo: TicketData;
    index: number;
    selectTicket: (ticket: TicketData) => void;
    isSelected: boolean;
}

function TicketTableRow({ ticketinfo, index, selectTicket, isSelected}: TicketTableRowProps) {
    const handleCheckboxChange = () => {
        selectTicket(ticketinfo);
    };

    const handleCheckboxClick = (event: any) => {
        // This prevens the row click from firing when clicking directly on the checkbox
        event.stopPropagation();
    };

    return (
        <tr className="row-taller" role="button"  onClick={handleCheckboxChange}>
            <td>
                <input className="form-check-input border-black"
                       type="checkbox"
                       value={ticketinfo.id.toString()}
                       id={ticketinfo.id.toString()}
                       checked={isSelected}
                       onClick={handleCheckboxClick}
                       onChange={handleCheckboxChange}
                />
            </td>
            <td>{index}</td>
            <td>{ticketinfo.issueType}</td>
            <td>{ticketinfo.description}</td>
            <td>{ticketinfo.status}</td>
        </tr>
    );
}

export default TicketTableRow;
