import {ToggleButton, ToggleButtonGroup} from "react-bootstrap";
import React from "react";

type TMatchLinksParams = {
    selectedMatchNav: number
    setSelectedMatchNav: React.Dispatch<React.SetStateAction<number>>
}

const MatchLinks = ({selectedMatchNav, setSelectedMatchNav} : TMatchLinksParams) => {
    const links: { id: number, value: string }[] = [{id: 0, value: "List"}, {id: 1, value: "My MatchList"}];
    return (
        <>
            <ToggleButtonGroup className="mt-3 col-12 col-md-auto" type="radio" name="options" defaultValue={0}>
                {
                    links.map((links: { id: number, value: string }) => {
                        const isSelected: boolean = selectedMatchNav === links.id;
                        return (
                            <ToggleButton
                                key={links.id}
                                id={`tbg-radio-${links.id}`}
                                className={`${isSelected ? "match-btn-active" : "match-btn-inactive"}`}
                                value={links.id}
                                onClick={() => setSelectedMatchNav(links.id)}>
                                {links.value}
                            </ToggleButton>
                        )
                    })
                }
            </ToggleButtonGroup>
        </>
    );
};

export default MatchLinks;
