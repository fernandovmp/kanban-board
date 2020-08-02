import React from 'react';
import { createGlobalStyle } from 'styled-components';
import { Routes } from './routes';

const GlobalStyles = createGlobalStyle`
    :root {
        --primary: #FFEA31;
        --background: #FAFAFA;
        --text: #323130;
    }

    * {
        box-sizing: border-box;
        font-family: 'Roboto', sans-serif;
        color: var(--text);
    }

    html,
    body {
        margin: 0;
        padding: 0;
    }

    html, body, #root {
        height: 100%;
    }

    #root {
        background: var(--background);
    }
`;

function App() {
    return (
        <>
            <GlobalStyles />
            <Routes />
        </>
    );
}

export default App;
