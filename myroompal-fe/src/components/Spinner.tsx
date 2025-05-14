import EnumSpinnerSize from "../models/EnumSpinnerSize";

function Spinner({ size }: { size: EnumSpinnerSize }) {
    let sizeClass = "";
    switch (size) {
        case EnumSpinnerSize.SM:
            sizeClass = "3";
            break;
        case EnumSpinnerSize.MD:
            sizeClass = "15";
            break;
        case EnumSpinnerSize.LG:
            sizeClass = "25";
            break;
        case EnumSpinnerSize.XL:
            sizeClass = "40";
            break;
        default:
            break;
    }

    return (
        <div className="text-darkblue spinner-grow" style={{ width: `${sizeClass}rem`, height: `${sizeClass}rem` }} role="status">
        </div>
    );
}

export default Spinner;
