<?php
// Process form on POST
$errors = [];
$result = null;
$a = $b = null;

if ($_SERVER['REQUEST_METHOD'] === 'POST') {
  $a = filter_input(INPUT_POST, 'a', FILTER_VALIDATE_FLOAT);
  $b = filter_input(INPUT_POST, 'b', FILTER_VALIDATE_FLOAT);

  if ($a === false || $b === false) {
    $errors[] = "Please enter valid numbers.";
  } else {
    $result = $a + $b;
  }
}
?>
<!doctype html>
<html lang="en">
<head>
  <meta charset="utf-8">
  <meta name="viewport" content="width=device-width, initial-scale=1">
  <title>Add Two Numbers (PHP)</title>
  <link rel="stylesheet" href="styles.css">
</head>
<body>
  <main class="card">
    <h1>Add Two Numbers (PHP)</h1>

    <?php if ($errors): ?>
      <div class="alert"><?php echo implode('<br>', array_map('htmlspecialchars', $errors)); ?></div>
    <?php endif; ?>

    <form method="post">
      <label for="a">First number</label>
      <input id="a" name="a" type="text" inputmode="decimal"
             value="<?php echo $a !== null ? htmlspecialchars((string)$a) : '' ?>">

      <label for="b">Second number</label>
      <input id="b" name="b" type="text" inputmode="decimal"
             value="<?php echo $b !== null ? htmlspecialchars((string)$b) : '' ?>">

      <button type="submit">Add</button>
    </form>

    <div class="result" aria-live="polite">
      <?php if ($result !== null): ?>
        <?php echo htmlspecialchars("$a + $b = $result"); ?>
      <?php endif; ?>
    </div>
  </main>

  <footer><small>PHP • HTML • CSS</small></footer>
</body>
</html>
