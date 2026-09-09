"""Prompt definitions for the Mesmerist mod's ability icons.

Each entry maps a blueprint name (which is also the PNG filename IconLoader looks up)
to the subject half of its prompt. STYLE is appended to every one so the whole set
reads as a single icon sheet rather than 59 unrelated paintings.

Keep subjects concrete and object-like. SDXL renders "a cracked obsidian mask" far more
reliably than "misdirection", and an icon has to be legible at 64 pixels, so one central
object on a plain ground beats a scene every time.
"""

# Shared across the whole set. The mesmerist's palette is violet/amethyst psychic light
# against desaturated dark stone, which also keeps the icons distinct from the base
# game's mostly warm-toned ability art.
# CLIP truncates at 77 tokens and the subject is prepended, so this stays short -
# anything past the limit is silently dropped. Framing negatives (no text, no border)
# live in NEGATIVE instead of being spent here.
STYLE = (
    "fantasy RPG ability icon, painterly, dark stone background, "
    "violet rim light, amethyst arcane glow, high contrast silhouette"
)

NEGATIVE = (
    "text, letters, watermark, signature, ui frame, border, multiple objects, "
    "cluttered, busy background, photorealistic, photograph, low contrast, blurry, "
    "washed out, cropped, collage, grid, person holding item, hands, "
    # The only palette guardrail worth keeping: one pilot icon came back crimson and orange,
    # which is the base game's warm register and the thing the violet set exists to avoid.
    # Everything else is left open on purpose - dictating composition here (centered object,
    # vignette, no scenes) and pushing guidance up made the set uniform but lifeless.
    "crimson, red, orange"
)

# --- Pilot batch: deliberately spans the different concept shapes in the mod ---
PILOT = {
    # Class signature - a gaze effect
    "HypnoticStare":
        "a single luminous human eye with a spiralling violet iris, hypnotic swirl, "
        "glowing amethyst light radiating from the pupil",

    # Offensive stare - damage
    "PainfulStare":
        "a cracked violet crystal eye splintering outward, shards of amethyst light, "
        "sharp fracture lines, sense of psychic pain",

    # Bold stare - defensive / cannot be flanked
    "Unaided":
        "a lone figure silhouette encircled by a ring of violet ward runes, "
        "protective circle, isolated and unflankable, glowing sigils",

    # Bold stare - fear
    "Nightmare":
        "a gaunt shadowed skull mask wreathed in violet mist, hollow glowing eye "
        "sockets, creeping dread, wisps of dark smoke",

    # Trick - teleport / movement
    "CompelAlacrity":
        "a violet portal rift torn in mid air with a blurred afterimage streaking "
        "through it, motion trails, sudden displacement",

    # Trick - forced movement / shove
    "LevitationBuffer":
        "a concentric shockwave of violet force expanding outward from a central point, "
        "ripples of energy pushing stones outward, kinetic burst",

    # Masterful trick - horror / stun
    "VisionOfBlood":
        "a shattered mirror shard reflecting a screaming face, crimson and violet light, "
        "horrifying vision, jagged glass",

    # Masterful trick - illusion / invisibility
    "FakedDeath":
        "a translucent ghostly figure dissolving into violet mist, fading silhouette, "
        "empty funeral shroud collapsing, vanishing",
}

# --- Remaining icons, filled in once the pilot locks the style ---
# Bold stares all keep an eye at the centre so the row reads as one family in the
# level-up list; tricks are object-led, since a trick is a thing you implant.
FULL = {
    # Bold stares
    "BoldStare":
        "a wide unblinking violet eye ringed by concentric arcane circles, "
        "intensified gaze, layered sigil rings",
    "Allure":
        "a hypnotic violet lotus blooming around a half lidded eye, entrancing "
        "beauty, drifting petals",
    "Disorientation":
        "a violet eye at the centre of a tilted broken compass rose, spinning "
        "needle, skewed crooked lines",
    "Disquiet":
        "a trembling violet eye with rippling concentric rings, vibrating blurred "
        "outline, unsettled nerves",
    "Distracted":
        "a violet eye splitting into three overlapping misaligned copies, divided "
        "attention, scattered focus",
    "Infiltration":
        "a violet keyhole shaped pupil set in a shadowed mask, silent intrusion, "
        "creeping tendrils of dark mist",
    "Lethality":
        "a violet eye impaled on a thin dagger point, lethal precision, a single "
        "drop of dark blood",
    "Manifold":
        "three violet eyes arranged in a triangle sharing one radiant beam, "
        "repeated gaze, multiplied focus",
    "Oscillation":
        "a violet eye smeared into a wavering horizontal blur, oscillating double "
        "image, unsteady vibration",
    "PsychicInception":
        "a violet eye opening inside a cracked stone skull's forehead, mind "
        "piercing, thought made visible",
    "SappedMagic":
        "a violet eye draining a guttering spell rune into itself, unravelling "
        "threads of magic, fading glyph",
    "Sensed":
        "a violet eye above a trail of glowing footprints, revealed presence, "
        "faint tracks across dark stone",
    "Sluggishness":
        "a violet eye behind a slow dripping strand of amber resin, thick viscous "
        "ooze, mired sluggish weight",
    "Susceptibility":
        "a violet eye reflected in an open cracked book, exposed thoughts, easily "
        "read, peeled back layers",
    "Timidity":
        "a violet eye above a bent and blunted sword blade, weakened blow, sapped "
        "strength, drooping metal",

    # Tricks
    "TrickSelection":
        "a violet thread tied in an intricate knot around a glowing bead, "
        "implanted suggestion, hypnotic bond",
    "AstoundingAvoidance":
        "a violet silhouette bending impossibly aside as a blast of light passes, "
        "evasive arc, near miss",
    "ConcealingVeil":
        "a rippling translucent violet veil half dissolving a figure, shimmering "
        "gauze, greater invisibility",
    "CursedSanction":
        "a violet curse sigil branded onto a floating blade, retribution mark, "
        "sickly glowing brand",
    "FalseFlanker":
        "a violet phantom duplicate standing behind a foe, illusory ally flanking, "
        "doubled silhouette",
    "FearsomeGuise":
        "a snarling violet demon mask overlaying a calm face, terrifying guise, "
        "bared fangs, aura of dread",
    "FleetInShadows":
        "a pair of violet winged boots trailing streaks of shadow, swift running, "
        "speed lines, blurred motion",
    "ForcedHope":
        "a violet war banner unfurling above a planted spear, surge of morale, "
        "radiant heraldic light, defiant",
    "FreeInBody":
        "shattered violet chains bursting apart in midair, snapping shackles, "
        "a broken manacle, unbound",
    "GiftOfWill":
        "a violet flame burning steady inside a mind shaped lantern, unbreakable "
        "will, calm resolute glow",
    "LinkedReaction":
        "a violet lightning arc leaping between two linked rings, instant "
        "reaction, quickened reflex spark",
    "MeekFacade":
        "a plain humble violet mask hiding a faint ward glow beneath, unassuming "
        "facade, subtle shimmer",
    "MesmericMirror":
        "a floating violet mirror shard reflecting a duplicate figure, illusory "
        "decoy, splintering image",
    "MesmericPantomime":
        "violet marionette strings descending onto an empty wooden puppet, mimed "
        "motion, puppeteer threads",
    "Misdirection":
        "a violet arrow curving sharply away from a figure turning the wrong way, "
        "misdirected strike, off balance",
    "PsychosomaticSurge":
        "a violet heart shaped ward pulsing with surging light, temporary "
        "vitality, radiant overflow",
    "ReflectFear":
        "a violet mirrored shield throwing back a wave of dark mist, repelled "
        "dread, reflected terror",
    "SeeInDarkness":
        "a violet eye drawn as radiating sonar rings in pitch darkness, "
        "blindsight, expanding echo pulses",
    "ShadowBlend":
        "a figure dissolving edge first into violet shadow, blending into "
        "darkness, vanishing outline",
    "ShadowSplinter":
        "jagged violet shadow shards orbiting and absorbing a strike, splintered "
        "darkness, blunted blow",
    "SlipBonds":
        "a violet knotted rope sliding loose into open coils, slipping free of "
        "bonds, unravelling knot",
    "SpectralSmoke":
        "coiling violet spectral smoke wreathing a dim silhouette, obscuring "
        "haze, drifting concealment",
    "UmbralShield":
        "a violet umbral disc eclipsing a harsh blazing light, shading shield, "
        "dark corona ring",
    "UnwittingMessanger":
        "a violet sealed letter glowing faintly in midair, hidden message, arcane "
        "wax seal, secret delivery",
    "VanishArrow":
        "a violet arrow disintegrating into motes in mid flight, deflected shaft, "
        "dispersing fragments",
    "VoiceOfReason":
        "concentric violet sound wave rings radiating from a still bell, "
        "steadying voice, resonant clarity",

    # Painful stare feats
    "BleedingStare":
        "a violet eye weeping thin ribbons of crimson blood, bleeding gaze, "
        "dripping wound, dark stains",
    "CompoundedPain":
        "two violet eye sigils overlapping into one brighter mark, compounded "
        "effect, layered stacking glow",
    "DemoralizingStare":
        "a violet eye above a shattered morale banner, crushing dread, torn "
        "cloth, sinking despair",
    "ExcoriatingStare":
        "a violet eye flaying strips of light from a stone figure, excoriating "
        "gaze, scoured peeling surface",
    "FatiguingStare":
        "a violet eye draining a slumping exhausted silhouette, sapped stamina, "
        "heavy fatigue, wilting",
    "IntensePain":
        "a violet eye at the core of a burst of radiating pain spikes, "
        "intensified agony, sharp lances",

    # Class features
    "ConsummateLiar":
        "a violet forked serpent tongue behind a smiling porcelain mask, silver "
        "tongued deceit, honeyed lie",
    "MentalPotency":
        "a violet brain shaped nebula crackling with amplified arcs, expanded "
        "mental power, growing corona",
    "ToweringEgo":
        "a towering violet crowned silhouette rising above small shadows, "
        "unshakable ego, proud radiant aura",
    "TouchTreatment":
        "a violet rune of cleansing light burning away dark clinging tendrils, "
        "psychic remedy, purified glow",
    "MasterfulTricks":
        "an ornate violet sigil ring of interlocking hypnotic knots, mastery, "
        "elaborate braided arcane bond",
    "ForceOfPersonality":
        "a violet crowned silhouette standing firm against a battering wave of mental "
        "force, unbroken will, radiant aura",
    "ExtraMesmeristTrick":
        "a violet knotted cord with one extra loop being drawn tight, an additional "
        "implanted bond, glowing thread",
    "ExtraBoldStare":
        "a violet eye with a second smaller eye opening beside it, a further gaze, "
        "layered sigil rings",
    "MythicAwesomeDisplay":
        "a violet starfield orrery bursting open in a dazzling cosmic display, "
        "wheeling constellations, awe",

    # Vexing Daredevil (archetype)
    "VexingDaredevil":
        "a violet rapier crossed with a wide unblinking eye, reckless swordplay "
        "married to hypnotic focus, dueling stance",
    "ImprovedFeint":
        "a violet blade feinting low while a shadow doppelganger strikes high, "
        "misdirected thrust, deceptive footwork",
    "GreaterFeint":
        "a violet blade leaving a lingering afterimage that pins an off balance "
        "silhouette, sustained opening, trailing light",
    "GreaterMesmerizingFeint":
        "a violet eye staring through a cracked animalistic mask mid feint, "
        "mind over instinct, hypnotic override",
    "DazzlingFeint":
        "a violet eye behind a starburst of dazzling light along a blade's edge, "
        "blinding riposte, radiant glare",
    "BlindingStrike":
        "a violet blade trailing a blinding starburst flash into an eye shaped "
        "silhouette, searing glare, sudden light",
    "CombatManeuver":
        "a violet blade hooking an off balance silhouette off its feet, "
        "exploited opening, controlled leverage",
    "CriticalStrike":
        "a violet blade driving into a fracturing weak point of light, precise "
        "follow-up, shattering impact",
    "Outmanuever":
        "a violet blurred footstep trail circling past a startled silhouette, "
        "swift repositioning, evasive arc",
    "PiercingStrike":
        "a violet blade piercing through a cracked psychic eye shaped ward, "
        "deepened wound, splintering light",
    "SloppyDefense":
        "a violet shield splintering open before an oncoming blade, exposed "
        "guard, crumbling defense",
    "SurpriseStrike":
        "two violet blade strikes in rapid succession trailing one motion blur, "
        "unseen follow-up, sudden second cut",
    "ShimmeringBody":
        "a violet silhouette dissolving into shimmering duplicate afterimages, "
        "blurred outline, wavering light",
}

ALL = {**PILOT, **FULL}
