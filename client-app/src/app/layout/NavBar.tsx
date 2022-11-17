import { observer } from "mobx-react-lite";
import React from "react";
import { NavLink } from "react-router-dom";
import { Button, Container, Menu } from "semantic-ui-react";

function NavBar() {

    return (
        <Menu inverted fixed="top">
            <Container>
                <Menu.Item header as={NavLink} to='/' exact>
                    <img src="/assets/logo.png" alt="logo" style={{marginRight: 10}} />
                    Reactivities
                </Menu.Item>
                <Menu.Item name="Activities" as={NavLink} to='/activities'>
                </Menu.Item>
                <Menu.Item>
                    <Button as={NavLink} to='/createActivity' positive content="Create Activity"></Button>
                </Menu.Item>
            </Container>
        </Menu>
    );
}

export default observer(NavBar)
