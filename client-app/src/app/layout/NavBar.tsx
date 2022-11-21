import { observer } from "mobx-react-lite";
import React from "react";
import { Link, NavLink } from "react-router-dom";
import { Button, Container, Dropdown, DropdownMenu, Image, Menu } from "semantic-ui-react";
import { useStore } from "../stores/store";

function NavBar() {
    const {userStore: {currentUser, logoutUser, isLoggedIn}} = useStore();

    return (
        <Menu inverted fixed="top">
            {(isLoggedIn) ? (
                <Container>
                    <Menu.Item header as={NavLink} to='/' exact>
                        <img src="/assets/logo.png" alt="logo" style={{marginRight: 10}} />
                        Reactivities
                    </Menu.Item>
                    <Menu.Item name="Activities" as={NavLink} to='/activities'>
                    </Menu.Item>
                    <Menu.Item name="Errors" as={NavLink} to='/errors'>
                    </Menu.Item>
                    <Menu.Item>
                        <Button as={NavLink} to='/createActivity' positive content="Create Activity"></Button>
                    </Menu.Item>
                    <Menu.Item position='right'>
                        <Image src={currentUser?.imageUrl || '/assets/user.png'} alt='User image' avatar spaced='right' />
                        <Dropdown pointing='top left' text={currentUser?.displayName} >
                            <DropdownMenu>
                                <Dropdown.Item as={Link} to={`/profile/${currentUser?.username}`} text='My Profile' icon='user' />
                                <Dropdown.Item onClick={logoutUser} text='Logout' icon='power' />
                            </DropdownMenu>
                        </Dropdown>
                    </Menu.Item>
                </Container>
            ) : (
                <Container></Container>
            )}
            
        </Menu>
    );
}

export default observer(NavBar)
