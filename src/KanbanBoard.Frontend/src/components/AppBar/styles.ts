import styled from 'styled-components';

export const AppBarHeader = styled.header`
    display: flex;
    background-color: var(--primary);
    align-items: center;
    justify-content: center;
    height: 50px;
    width: 100%;
    box-shadow: 0px 2px 4px rgba(0, 0, 0, 0.25);
`;

export const ToolBar = styled.div`
    flex: 1;
    display: flex;
    justify-content: flex-end;
    margin-right: 24px;
`;

export const AppName = styled.strong`
    font-size: large;
`;

export const LogOutIcon = styled.img`
    :hover {
        cursor: pointer;
    }
`;
