
-- 🎭 Жанр
CREATE TABLE genre (
                       id SERIAL PRIMARY KEY,
                       name TEXT NOT NULL UNIQUE
);

-- 🎬 Фильм
CREATE TABLE movie (
                       id SERIAL PRIMARY KEY,
                       title TEXT NOT NULL,
                       description TEXT,
                       duration_minutes INT NOT NULL,
                       rating REAL CHECK (rating >= 0 AND rating <= 10),
                       poster_url TEXT
);

-- 🎬 Жанры фильма
CREATE TABLE movie_genre(
    id SERIAL PRIMARY KEY,
    genre_id int  REFERENCES genre(id) ON DELETE CASCADE,
    movie_id int  REFERENCES movie(id) ON DELETE CASCADE
);

-- 🏛️ Зал
CREATE TABLE hall (
                      id SERIAL PRIMARY KEY,
                      name TEXT,
                      rows INT NOT NULL CHECK (rows > 0),
    seats_per_row INT NOT NULL CHECK (seats_per_row > 0)
);

-- 🕒 Сеанс
CREATE TABLE session (
                         id SERIAL PRIMARY KEY,
                         movie_id INT REFERENCES movie(id) ON DELETE CASCADE,
                         hall_id INT REFERENCES hall(id) ON DELETE CASCADE,
                         start_time TIMESTAMP NOT NULL,
                         end_time TIMESTAMP NOT NULL,
                         base_price DECIMAL(8,2) NOT NULL CHECK (base_price >= 0)
);

CREATE TABLE seat_reservation (
                                  id SERIAL PRIMARY KEY,
                                  session_id INT NOT NULL REFERENCES session(id) ON DELETE CASCADE,
                                  row_number INT NOT NULL CHECK (row_number >= 0),
                                  seat_number INT NOT NULL CHECK (seat_number >= 0),
                                  reserved_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
                                  CONSTRAINT unique_seat_per_session UNIQUE (session_id, row_number, seat_number)
);
