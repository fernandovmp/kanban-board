import styled from 'styled-components';

export const Main = styled.main`
    padding: 0 60px;
`;

export const Header = styled.header`
    display: flex;
    flex-wrap: wrap;
    align-items: center;
    gap: 10px;
`;

export const Button = styled.button`
    background: #f0f0f0;
    border: none;
    border-radius: 4px;
    display: flex;
    align-items: center;
    gap: 4px;
    padding: 4px 6px;
    font-weight: bold;
    font-size: medium;

    &:hover {
        cursor: pointer;
    }

    &:focus {
        outline: none;
        background: #e0e0e0;
    }
`;

export const BoardTitle = styled.h2`
    margin-right: 50px;
`;
