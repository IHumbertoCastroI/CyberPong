const canvas = document.getElementById('gameCanvas');
const ctx = canvas.getContext('2d');

// Constantes do jogo
const paddleWidth = 10;
const paddleHeight = 100;
const ballRadius = 10;
const paddleSpeed = 5;
const ballSpeed = 3;

// Estados do jogo
let playerPaddle = { x: 0, y: canvas.height / 2 - paddleHeight / 2 };
let aiPaddle = { x: canvas.width - paddleWidth, y: canvas.height / 2 - paddleHeight / 2 };
let ball = { x: canvas.width / 2, y: canvas.height / 2, dx: ballSpeed, dy: ballSpeed };

// Controle do jogador
document.addEventListener('keydown', movePaddle);

function movePaddle(e) {
    if (e.key === 'ArrowUp') {
        playerPaddle.y = Math.max(playerPaddle.y - paddleSpeed, 0);
    } else if (e.key === 'ArrowDown') {
        playerPaddle.y = Math.min(playerPaddle.y + paddleSpeed, canvas.height - paddleHeight);
    }
}

// Função para desenhar o retângulo
function drawRect(x, y, width, height, color) {
    ctx.fillStyle = color;
    ctx.fillRect(x, y, width, height);
}

// Função para desenhar a bola
function drawBall(x, y, radius, color) {
    ctx.fillStyle = color;
    ctx.beginPath();
    ctx.arc(x, y, radius, 0, Math.PI * 2);
    ctx.closePath();
    ctx.fill();
}

// Função para atualizar a posição da bola
function updateBall() {
    ball.x += ball.dx;
    ball.y += ball.dy;

    // Verificar colisão com paredes superior e inferior
    if (ball.y + ballRadius > canvas.height || ball.y - ballRadius < 0) {
        ball.dy = -ball.dy;
    }

    // Verificar colisão com as raquetes
    if (ball.x - ballRadius < paddleWidth) {
        if (ball.y > playerPaddle.y && ball.y < playerPaddle.y + paddleHeight) {
            ball.dx = -ball.dx;
        } else {
            resetBall();
        }
    }

    if (ball.x + ballRadius > canvas.width - paddleWidth) {
        if (ball.y > aiPaddle.y && ball.y < aiPaddle.y + paddleHeight) {
            ball.dx = -ball.dx;
        } else {
            resetBall();
        }
    }
}

// Função para resetar a bola
function resetBall() {
    ball.x = canvas.width / 2;
    ball.y = canvas.height / 2;
    ball.dx = ballSpeed;
    ball.dy = ballSpeed;
}

// Função para desenhar a tela do jogo
function draw() {
    ctx.clearRect(0, 0, canvas.width, canvas.height);
    drawRect(playerPaddle.x, playerPaddle.y, paddleWidth, paddleHeight, '#fff');
    drawRect(aiPaddle.x, aiPaddle.y, paddleWidth, paddleHeight, '#fff');
    drawBall(ball.x, ball.y, ballRadius, '#fff');
}

// Função para atualizar a raquete do AI
function updateAiPaddle() {
    if (aiPaddle.y + paddleHeight / 2 < ball.y) {
        aiPaddle.y = Math.min(aiPaddle.y + paddleSpeed, canvas.height - paddleHeight);
    } else {
        aiPaddle.y = Math.max(aiPaddle.y - paddleSpeed, 0);
    }
}

// Função principal do jogo
function gameLoop() {
    updateBall();
    updateAiPaddle();
    draw();
    requestAnimationFrame(gameLoop);
}

gameLoop();

// Função para obter jogadores
async function fetchPlayers() {
    try {
        const response = await axios.get('/players');
        console.log(response.data);
    } catch (error) {
        console.error('Error fetching players:', error);
    }
}

// Função para enviar pontuações
async function postScore(score) {
    try {
        const response = await axios.post('/scores', score, {
            headers: {
                'Content-Type': 'application/json',
            },
        });
        console.log(response.data);
    } catch (error) {
        console.error('Error posting score:', error);
    }
}

// Chamar as funções para teste
fetchPlayers();
postScore({ playerId: 1, points: 10 });
